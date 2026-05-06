using ApiContinental.Application.DTOs;
using ApiContinental.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiContinental.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImcController : ControllerBase
    {
        private readonly IImcService _imcService;

        public ImcController(IImcService imcService)
        {
            _imcService = imcService;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Calculate([FromBody] ImcRequestDto dto)
        {
            var result = await _imcService.CalculateAsync(dto);
            return Ok(result);
        }
    }
}
