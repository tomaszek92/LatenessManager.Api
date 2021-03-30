using System.Text;
using LatenessManager.Application.Abstractions;
using LatenessManager.Application.Common.Models;
using LatenessManager.Application.Identity.Abstractions;
using LatenessManager.Infrastructure.Configurations;
using LatenessManager.Infrastructure.Identity;
using LatenessManager.Infrastructure.Persistence;
using LatenessManager.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace LatenessManager.Infrastructure
{
    public static class DependencyInjection
    {
        public const string AllowSpecificOrigins = "AllowSpecificOrigins";

        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("LatenessManagerDb"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IDomainEventService, DomainEventService>();

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddSingleton<IJwtHandler, JwtHandler>();
            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddSingleton<ICurrentUserProvider, CurrentUserProvider>();

            services.AddConfigurationSingleton<FacebookConfiguration>(configuration);
            services.AddHttpClient<IFacebookClient, FacebookClient>();
            services.AddSingleton<IFacebookPublisher, FacebookPublisher>();

            var corsConfiguration = services.AddConfigurationSingleton<CorsConfiguration>(configuration);

            services.AddCors(options =>
            {
                options.AddPolicy(
                    AllowSpecificOrigins,
                    builder => builder
                        .WithOrigins(corsConfiguration.AllowedHosts.Split(','))
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            
            var jwtTokenConfiguration = services.AddConfigurationSingleton<JwtTokenConfiguration>(configuration);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var secretBytes = Encoding.ASCII.GetBytes(jwtTokenConfiguration.Secret);
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireExpirationTime = false,
                        ValidateLifetime = true
                    };
                });

            return services;
        }

        private static T AddConfigurationSingleton<T>(this IServiceCollection services, IConfiguration configuration) 
            where T : class
        {
            var instance = configuration.GetSection(typeof(T).Name).Get<T>();
            services.AddSingleton(instance);

            return instance;
        }
    }
}