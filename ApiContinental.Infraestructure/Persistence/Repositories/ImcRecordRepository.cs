using ApiContinental.Application.Interfaces;
using ApiContinental.Infraestructure.Configurations.Contexts;
using Microsoft.EntityFrameworkCore;
using ApiContinental.Domain.Entities;

namespace ApiContinental.Infraestructure.Persistence.Repositories
{
    public class ImcRecordRepository : IImcRecordRepository
    {
        private readonly AppDbContext _dbContext;

        public ImcRecordRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Add(ImcRecord record)
        {
            await _dbContext.ImcRecords.AddAsync(record);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
