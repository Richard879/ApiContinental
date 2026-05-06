using ApiContinental.Application.Interfaces;
using ApiContinental.Application.Services;
using ApiContinental.Infraestructure.Configurations.Contexts;
using ApiContinental.Infraestructure.KeyVault;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ApiContinental.Infraestructure;

var builder = WebApplication.CreateBuilder(args);

// Logging temprano
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Añadir Key Vault al Configuration (usa Managed Identity / DefaultAzureCredential)
builder.Configuration.AddAzureKeyVaultFromEnvironment(builder.Configuration);

builder.Services.AddInfrastructureServices(builder.Configuration);

// Autenticación JWT (clave en Key Vault: JwtSecret)
var jwtSecret = builder.Configuration["JwtSecret"];
if (string.IsNullOrEmpty(jwtSecret))
{
    // En entornos de desarrollo podríamos tener fallback; en producción asegúrate de crear el secreto JwtSecret en Key Vault.
    if (builder.Environment.IsDevelopment())
    {
        jwtSecret = "dev_jwt_secret_change_me_2026!";
        var tmpLogger = LoggerFactory.Create(lb => lb.AddConsole()).CreateLogger("Startup");
        tmpLogger.LogWarning("No se encontró 'JwtSecret'. Usando secreto JWT de desarrollo.");
    }
    else
    {
        throw new InvalidOperationException("No se encontró 'JwtSecret' en Key Vault.");
    }
}

var key = Encoding.UTF8.GetBytes(jwtSecret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiContinental Web API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticación JWT. Escribe: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            new string[] {}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("allowed", policy =>
    {
        var corsOrigins = builder.Configuration["Cors:AllowedOrigins"]
            ?? throw new InvalidOperationException("The Cors__AllowedOrigins environment variable is not set");

        if (corsOrigins == "*")
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
        else
        {
            var allowedOrigins = corsOrigins.Split(';', StringSplitOptions.RemoveEmptyEntries);
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("allowed");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

