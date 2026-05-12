using ApiContinental.Infraestructure.Configurations.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ApiContinental.Application.Interfaces;
using ApiContinental.Application.Services;
using ApiContinental.Infraestructure.Persistence.Repositories;

namespace ApiContinental.Infraestructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["DbConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("No se encontró 'DbConnectionString' en la configuración (Key Vault).");
            }

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Agregar DbContext EF con SQL
            //services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("CnxBDUSAT")));
            services.AddScoped<IImcService, ImcService>(); 
            services.AddScoped<IImcCategoriesRepository, ImcCategoriesRepository>();
            services.AddScoped<IImcRecordRepository, ImcRecordRepository>();
            return services;
        }
    }
}
