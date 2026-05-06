using ApiContinental.Application.DTOs;

namespace ApiContinental.Application.Interfaces
{
    public interface IImcService
    {
        Task<ImcResponseDto> CalculateAsync(ImcRequestDto request);
    }
}
