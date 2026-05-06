using ApiContinental.Application.DTOs;
using ApiContinental.Application.Interfaces;
using ApiContinental.Domain.Entities;
using ApiContinental.Infraestructure.Configurations.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ApiContinental.Application.Services
{
    public class ImcService : IImcService
    {
        private readonly AppDbContext _dbContext;

        public ImcService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
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

            // buscar categoría parametrizada
            if (age <= 19)
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
                    _dbContext.ImcRecords.Add(record);
                    await _dbContext.SaveChangesAsync();
                }

                return response;
            }

            // Para adultos (>19) buscar categoría en DB
            var category = await _dbContext.ImcCategories
                .FirstOrDefaultAsync(c =>
                    c.MinAge <= age && age <= c.MaxAge &&
                    c.MinImc <= imc && imc < c.MaxImc);

            string desc;
            if (category != null) desc = category.Description;
            else
            {
                // Si no existe parametrización, usar reglas WHO por defecto
                desc = GetWhoDescription(imc);
            }

            var resultDto = new ImcResponseDto
            {
                ImcValue = imc,
                Age = age,
                Description = desc
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
                    ImcDescription = desc,
                    CreatedAt = DateTime.UtcNow
                };
                _dbContext.ImcRecords.Add(record);
                await _dbContext.SaveChangesAsync();
            }

            return resultDto;
        }

        private string GetWhoDescription(decimal imc)
        {
            // WHO adultos (ejemplo):
            // <18.5 Bajo peso
            // 18.5–24.9 Normal
            // 25.0–29.9 Sobrepeso
            // >=30 Obesidad
            if (imc < 18.5m) return "Bajo peso";
            if (imc < 25m) return "Normal";
            if (imc < 30m) return "Sobrepeso";
            return "Obesidad";
        }
    }
}
