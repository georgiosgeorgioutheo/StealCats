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
        Task<List<CatEntity>> FetchCatImagesAsync(int limit = 25,  int hasBreeds = 1);
    }
}
