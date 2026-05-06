using ApiContinental.Application.Interfaces;
using ApiContinental.Infraestructure.Configurations.Contexts;
using Microsoft.EntityFrameworkCore;
using ApiContinental.Domain.Entities;

namespace ApiContinental.Infraestructure.Persistence.Repositories
{
    public class ImcCategoriesRepository : IImcCategoriesRepository
    {
        private readonly AppDbContext _context;

        public ImcCategoriesRepository(AppDbContext context)
        {
            _context = context;
        }   

        public async Task<IEnumerable<ImcCategory>> GetAllAsync()
        {
            return await _context.ImcCategories.ToListAsync();
        }   

        public async Task<ImcCategory> Get(Guid id)
        {
            return await _context.ImcCategories.FindAsync(id);
        }

        public async Task<bool> Create(ImcCategory category)
        {
            category.Id = Guid.NewGuid();
            _context.ImcCategories.Add(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(ImcCategory category)
        {
            var exist = await _context.ImcCategories.FindAsync(category.Id);
            if (exist == null) return false;
            exist.MinAge = category.MinAge;
            exist.MaxAge = category.MaxAge;
            exist.MinImc = category.MinImc;
            exist.MaxImc = category.MaxImc;
            exist.Description = category.Description;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(Guid id)
        {
            var exist = await _context.ImcCategories.FindAsync(id);
            if (exist == null) return false;
            _context.ImcCategories.Remove(exist);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}