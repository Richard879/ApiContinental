using ApiContinental.Domain.Entities;

namespace ApiContinental.Application.Interfaces
{
    public interface IImcCategoriesRepository
    {
        Task<IEnumerable<ImcCategory>> GetAllAsync();
        Task<ImcCategory> Get(Guid id);
        Task<bool> Create(ImcCategory category);
        Task<bool> Update(ImcCategory category);
        Task<bool> Delete(Guid id);
    }
}
