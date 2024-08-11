using Core.Entities;

namespace Core.Interfaces
{
    public  interface IStealCatApiService
    {
        Task<List<CatEntity>> StealCatsAsync(int limit = 25,  int hasBreeds = 1);
    }
}
