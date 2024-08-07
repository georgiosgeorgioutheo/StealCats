using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(c => c.CatTags).ThenInclude(ct => ct.Tag)
                .ToListAsync();
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
                .Where(c => c.CatTags.Any(ct => ct.Tag.Name == tag))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(c => c.CatTags).ThenInclude(ct => ct.Tag)
                .ToListAsync();
        }

        public async Task AddCatsAsync(IEnumerable<CatEntity> cats)
        {
            foreach (var cat in cats)
            {
                var existingCat = await _context.Cats
                    .Include(c => c.CatTags).ThenInclude(ct => ct.Tag)
                    .FirstOrDefaultAsync(c => c.CatId == cat.CatId);

                if (existingCat == null)
                {
                    await AddNewCatAsync(cat);
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task AddNewCatAsync(CatEntity cat)
        {
            cat.Created = DateTime.UtcNow;

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

