/*using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation.AspNetCore;
using FluentValidation;
using Application.Users;

namespace WebAPI.Extensions
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection AddGenerationServices(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddDbContext<GenerationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("GenerationDatabase"),
                    b => b.MigrationsAssembly("Infrastructure") 
                ));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
            };
        });

            services.AddScoped<IGenerationUnitOfWork, GenerationUnitOfWork>();

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();
            services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
            services.AddScoped<IJwtProvider, JwtProvider>();

            return services;
        }
    }
}
*/