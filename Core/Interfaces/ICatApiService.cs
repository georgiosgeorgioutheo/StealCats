using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public  interface ICatApiService
    {
        Task<List<CatEntity>> FetchCatImagesAsync(int limit = 25, int page = 0, string order = "RAND", int hasBreeds = 1, string breedIds = null, string categoryIds = null, string subId = null);
    }
}
