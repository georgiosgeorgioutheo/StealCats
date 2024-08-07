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
                if (!_context.Cats.Any(c => c.CatId == cat.CatId))
                {
                    cat.Created = DateTime.UtcNow;

                    // Add tags and relations
                    foreach (var catTag in cat.CatTags)
                    {
                        var existingTag = await _context.Tags
                            .FirstOrDefaultAsync(t => t.Name == catTag.Tag.Name);
                        if (existingTag == null)
                        {
                            _context.Tags.Add(catTag.Tag);
                        }
                        else
                        {
                            catTag.TagId = existingTag.Id; // Use existing tag ID
                        }

                        _context.CatTags.Add(new CatTagEntity
                        {
                            Cat = cat,
                            Tag = existingTag ?? catTag.Tag
                        });
                    }

                    _context.Cats.Add(cat);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}

