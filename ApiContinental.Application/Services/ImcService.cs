using ApiContinental.Application.DTOs;
using ApiContinental.Application.Interfaces;
using ApiContinental.Domain.Entities;

namespace ApiContinental.Application.Services
{
    public class ImcService : IImcService
    {
        private readonly IImcRecordRepository _recordRepository;
        private readonly IImcCategoriesRepository _categoriesRepository;

        public ImcService(IImcRecordRepository recordRepository, IImcCategoriesRepository categoriesRepository)
        {
            _recordRepository = recordRepository;
            _categoriesRepository = categoriesRepository;
        }

        public async Task<ImcResponseDto> CalculateAsync(ImcRequestDto request)
        {
            // calcular edad
            var today = DateTime.UtcNow.Date;
            var dob = request.DateOfBirth.Date;
            var age = today.Year - dob.Year;
            if (dob > today.AddYears(-age)) age--;

            // convertir altura a metros
            var heightMeters = request.HeightCm / 100m;
            if (heightMeters <= 0) throw new ArgumentException("Altura inválida.");

            // IMC = peso(kg) / altura(m)^2
            var imc = Math.Round(request.WeightKg / (heightMeters * heightMeters), 2);

            var category = await _categoriesRepository.GetCategoryForImcAsync(age, imc);

            string desc;
            if (category != null)
            {
                desc = category.Description;

                var resultDto = new ImcResponseDto
                {
                    ImcValue = imc,
                    Age = age,
                    Description = desc
                };

                var record = new ImcRecord
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    WeightKg = request.WeightKg,
                    HeightCm = request.HeightCm,
                    DateOfBirth = request.DateOfBirth,
                    Age = age,
                    ImcValue = imc,
                    ImcDescription = desc,
                    CreatedAt = DateTime.UtcNow
                };

                if (request.Persist)
                {
                    await _recordRepository.Add(record);
                }

                return resultDto;
            }
            else
            {
                var response = new ImcResponseDto
                {
                    ImcValue = imc,
                    Age = age,
                    Description = "No se han registrado los percentiles para niños y adolescentes"
                };

                if (request.Persist)
                {
                    var record = new ImcRecord
                    {
                        Id = Guid.NewGuid(),
                        Name = request.Name,
                        WeightKg = request.WeightKg,
                        HeightCm = request.HeightCm,
                        DateOfBirth = request.DateOfBirth,
                        Age = age,
                        ImcValue = imc,
                        ImcDescription = response.Description,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _recordRepository.Add(record);
                }
                return response;
            }
        }
    }
}   
