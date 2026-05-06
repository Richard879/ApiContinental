using ApiContinental.Domain.Entities;
using ApiContinental.Infraestructure.Configurations.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiContinental.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImcCategoriesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ImcCategoriesController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _db.ImcCategories.ToListAsync());

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var cat = await _db.ImcCategories.FindAsync(id);
            if (cat == null) return NotFound();
            return Ok(cat);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ImcCategory category)
        {
            category.Id = Guid.NewGuid();
            _db.ImcCategories.Add(category);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ImcCategory category)
        {
            var exist = await _db.ImcCategories.FindAsync(id);
            if (exist == null) return NotFound();
            exist.MinAge = category.MinAge;
            exist.MaxAge = category.MaxAge;
            exist.MinImc = category.MinImc;
            exist.MaxImc = category.MaxImc;
            exist.Description = category.Description;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var exist = await _db.ImcCategories.FindAsync(id);
            if (exist == null) return NotFound();
            _db.ImcCategories.Remove(exist);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}