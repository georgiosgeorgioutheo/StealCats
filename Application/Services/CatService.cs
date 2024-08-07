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
        private readonly ICatApiService _catApiService;

        public CatService(ICatRepository catRepository, ICatApiService catApiService)
        {
            _catRepository = catRepository;
            _catApiService = catApiService;
        }

        public async Task FetchAndStoreCatsAsync(int limit = 25, int hasBreeds = 1 )
        {
            var cats = await _catApiService.FetchCatImagesAsync(limit,  hasBreeds);
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

