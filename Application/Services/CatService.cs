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
        private readonly IStealCatApiService _stealCatApiService;

        public CatService(ICatRepository catRepository, IStealCatApiService catApiService)
        {
            _catRepository = catRepository;
            _stealCatApiService = catApiService;
        }

        public async Task StoreCatsAsync(IEnumerable<CatEntity> cats)
        {
            await _catRepository.AddCatsAsync(cats);
        }

        public Task<CatEntity> GetCatByIdAsync(int id)
        {
            return _catRepository.GetCatByIdAsync(id);
        }

        public Task<IEnumerable<CatEntity>> GetCatsAsync(int page, int pageSize)
        {
            return _catRepository.GetCatsAsync(page, pageSize);
        }

        public Task<IEnumerable<CatEntity>> GetCatsByTagAsync(string tag, int page, int pageSize)
        {
            return _catRepository.GetCatsByTagAsync(tag, page, pageSize);
        }
    }
}

