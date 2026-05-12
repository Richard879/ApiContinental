using ApiContinental.Domain.Entities;

namespace ApiContinental.Application.Interfaces
{
    public interface IImcRecordRepository
    {
        Task<bool> Add(ImcRecord record);
    }
}