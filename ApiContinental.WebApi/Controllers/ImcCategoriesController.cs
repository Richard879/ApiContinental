using ApiContinental.Application.Interfaces;
using ApiContinental.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApiContinental.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImcCategoriesController : ControllerBase
    {
        private readonly IImcCategoriesRepository _imcCategoriesRepository;

        public ImcCategoriesController(IImcCategoriesRepository imcCategoriesRepository)
        {
            _imcCategoriesRepository = imcCategoriesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _imcCategoriesRepository.GetAllAsync();
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var cat = await _imcCategoriesRepository.Get(id);
            if (cat == null) return NotFound();
            return Ok(cat);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ImcCategory category)
        {
            category.Id = Guid.NewGuid();
            await _imcCategoriesRepository.Create(category);
            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ImcCategory category)
        {
            var exist = await _imcCategoriesRepository.Get(id);
            if (exist == null) return NotFound();
            exist.MinAge = category.MinAge;
            exist.MaxAge = category.MaxAge;
            exist.MinImc = category.MinImc;
            exist.MaxImc = category.MaxImc;
            exist.Description = category.Description;
            await _imcCategoriesRepository.Update(exist);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var exist = await _imcCategoriesRepository.Get(id);
            if (exist == null) return NotFound();
            await _imcCategoriesRepository.Delete(id);
            return NoContent();
        }
    }
}