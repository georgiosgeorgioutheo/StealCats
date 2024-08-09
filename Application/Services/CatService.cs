using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CatService
    {
        private readonly ICatRepository _catRepository;

        public CatService(ICatRepository catRepository)
        {
            _catRepository = catRepository;
        }

        public async Task<AddedCatsResult> StoreCatsAsync(IEnumerable<CatEntity> cats)
        {
            AddedCatsResult result = new AddedCatsResult();
            result = await _catRepository.AddCatsAsync(cats);

            return result;
        }

        public async Task<CatEntity> GetCatByIdAsync(int id)
        {
            return await _catRepository.GetCatByIdAsync(id);
        }

        public async Task<IEnumerable<CatEntity>> GetCatsAsync(int page, int pageSize)
        {
            return await _catRepository.GetCatsAsync(page, pageSize);
        }

        public async Task<IEnumerable<CatEntity>> GetCatsByTagAsync(string tag, int page, int pageSize)
        {
            return await _catRepository.GetCatsByTagAsync(tag, page, pageSize);
        }
    }
}

