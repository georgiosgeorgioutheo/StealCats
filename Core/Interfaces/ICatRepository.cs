using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces
{
    public interface ICatRepository
    {

        Task<IEnumerable<CatEntity>> GetCatsAsync(int page, int pageSize);
        Task<CatEntity> GetCatByIdAsync(int id);
        Task<IEnumerable<CatEntity>> GetCatsByTagAsync(string tag, int page, int pageSize);
        Task<AddedCatsResult> AddCatsAsync(IEnumerable<CatEntity> cats);
    }
}
