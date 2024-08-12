using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CatRepository : ICatRepository
    {
        private readonly CatContext _context;

        public CatRepository(CatContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CatEntity>> GetCatsAsync(int page, int pageSize)
        {
            return await _context.Cats
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(c => c.CatTags).ThenInclude(ct => ct.Tag)
                .ToListAsync();
        }
      
        public async Task<PageResponseDto> GetPageResponseDto(int currentPage,int pageSize, string tag)
        {
            PageResponseDto pageResponseDto = new PageResponseDto();
            var catsQuery = _context.Cats.AsQueryable();

            if (!string.IsNullOrEmpty(tag))
            {
                catsQuery = catsQuery.Where(c => c.CatTags.Any(ct => ct.Tag.Name == tag));
            }

            int totalCats = await catsQuery.CountAsync();
            pageResponseDto.TotalCount = totalCats;

            // Calculate the total number of pages
            var totalPages = (int)Math.Ceiling(totalCats / (double)pageSize);
            pageResponseDto.PageCount = totalPages;
            pageResponseDto.CurrentPage = currentPage;
            pageResponseDto.PageSize = pageSize;
            pageResponseDto.PageInfo = SetPageString(currentPage, totalPages);
            return pageResponseDto;
        }
        public static string SetPageString(int currentPage, int totalPages)
        {
            return $"Page {currentPage} of {totalPages}";

        }
        public async Task<CatEntity>? GetCatByIdAsync(int id)
        {
            return await _context.Cats
                .Include(c => c.CatTags).ThenInclude(ct => ct.Tag)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<CatEntity>> GetCatsByTagAsync(string tag, int page, int pageSize)
        {
            return await _context.Cats
                .AsNoTracking()
                .Where(c => c.CatTags.Any(ct => ct.Tag.Name == tag))
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(c => c.CatTags).ThenInclude(ct => ct.Tag)
                .ToListAsync();
        }

        public async Task<AddedCatsResult> AddCatsAsync(IEnumerable<CatEntity> cats)
        {
            AddedCatsResult addedCatsResult = new AddedCatsResult();
            foreach (var cat in cats)
            {
                var existingCat = await _context.Cats
                    .FirstOrDefaultAsync(c => c.CatId == cat.CatId);

                if (existingCat == null)
                {
                    await AddNewCatAsync(cat);
                    addedCatsResult.AddedCount++;
                }
                else
                {
                    await UpdateCatAsync(cat);
                    addedCatsResult.UpdatedCount++;
                }
            }

            await _context.SaveChangesAsync();
            return addedCatsResult;
        }

        public async Task UpdateCatAsync(CatEntity cat)
        {
            var existingCat = await _context.Cats
             .Include(c => c.CatTags)
             .ThenInclude(ct => ct.Tag)
             .FirstOrDefaultAsync(c => c.CatId == cat.CatId);

            if (existingCat == null)
            {
                throw new InvalidOperationException("Cat not found for update.");
            }

            
            existingCat.Width = cat.Width;
            existingCat.Height = cat.Height;
            existingCat.Image = cat.Image;
            existingCat.Created = DateTime.UtcNow; 

            // Update the tags
            _context.CatTags.RemoveRange(existingCat.CatTags); // Remove existing tags
            existingCat.CatTags.Clear(); // Clear the tags list

            foreach (var newTag in cat.CatTags)
            {
                await CheckAndAddTagAsync(newTag);
                existingCat.CatTags.Add(newTag);
            }
            await _context.SaveChangesAsync();
        }

        private async Task AddNewCatAsync(CatEntity cat)
        {
           
            cat.Created = DateTime.Now;

            foreach (var catTag in cat.CatTags)
            {
                await CheckAndAddTagAsync(catTag);
            }

            _context.Cats.Add(cat);
        }

       

        private async Task CheckAndAddTagAsync(CatTagEntity catTag)
        {
            var existingTag = await _context.Tags
                .FirstOrDefaultAsync(t => t.Name == catTag.Tag.Name);

            if (existingTag == null)
            {
                _context.Tags.Add(catTag.Tag);
                await _context.SaveChangesAsync(); // Save changes to get the ID
            }
            else
            {
                catTag.Tag = existingTag;
                catTag.TagId = existingTag.Id;
            }
        }

    }
}

