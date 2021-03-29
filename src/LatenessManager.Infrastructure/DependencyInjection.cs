using LatenessManager.Application.Abstractions;
using LatenessManager.Infrastructure.Persistence;
using LatenessManager.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LatenessManager.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("LatenessManagerDb"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IDomainEventService, DomainEventService>();

            return services;
        }
    }
}