using Azure.Identity;
using Microsoft.Extensions.Configuration;
using System;

namespace ApiContinental.Infraestructure.KeyVault
{
    public static class ConfigurationExtensions
    {
        public static IConfigurationBuilder AddAzureKeyVaultFromEnvironment(this IConfigurationBuilder builder, IConfiguration configuration)
        {
            //var vaultUri = Environment.GetEnvironmentVariable("AZURE_KEYVAULT_URI"); 
            var vaultUri = configuration["AZURE_KEYVAULT_URI"];
            if (string.IsNullOrEmpty(vaultUri))
            {
                // no hay Key Vault configurado: devuelve builder para permitir pruebas locales con appsettings
                return builder;
            }

            var credential = new DefaultAzureCredential();
            builder.AddAzureKeyVault(new Uri(vaultUri), credential);
            return builder;
        }
    }
}