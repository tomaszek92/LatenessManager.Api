using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using LatenessManager.Api;
using LatenessManager.Application.Abstractions;
using LatenessManager.Domain.Common;
using LatenessManager.Infrastructure.Persistence;
using LatenessManager.Tests.Common;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace LatenessManager.Application.IntegrationsTests
{
    [Collection("Integration tests")]
    public class TestBase : IAsyncLifetime
    {
        protected readonly IFixture Fixture = TestFixture.Get();
        protected readonly DateTime UtcNow;

        private static IConfigurationRoot _configuration;
        private static IServiceScopeFactory _scopeFactory;

        protected TestBase()
        {
            var dateTime = Fixture.Create<DateTime>();
            UtcNow = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }
        
        public Task InitializeAsync()
        {
            _configuration = GetConfigurationBuilder().Build();

            var startup = new Startup(_configuration);
            var services = new ServiceCollection();

            services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
                w.EnvironmentName == "Development" &&
                w.ApplicationName == "LatenessManager.Api"));

            services.AddLogging();

            startup.ConfigureServices(services);

            ReplaceDateTimeProvider(services);

            _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();

            EnsureDatabase();

            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<IApplicationDbContext>();

            if (context is null)
            {
                throw new Exception($"{nameof(IApplicationDbContext)} is null");
            }

            context.Players.RemoveRange(context.Players);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        private static IConfigurationBuilder GetConfigurationBuilder() =>
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

        private void ReplaceDateTimeProvider(IServiceCollection services)
        {
            var currentUserServiceDescriptor = services.FirstOrDefault(serviceDescriptor =>
                serviceDescriptor.ServiceType == typeof(IDateTimeProvider));

            services.Remove(currentUserServiceDescriptor);

            // Register testing version
            services.AddSingleton(_ =>
                Mock.Of<IDateTimeProvider>(dateTimeProvider => dateTimeProvider.UtcNow == UtcNow));
        }

        private static void EnsureDatabase()
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<IApplicationDbContext>();

            context?.Database.Migrate();
        }

        protected static async Task AddAsync<TEntity>(params TEntity[] entities)
            where TEntity : IAggregateRoot
        {
            using var scope = _scopeFactory.CreateScope();

            var applicationDbContext = GetApplicationDbContext(scope);

            foreach (var entity in entities)
            {
                await applicationDbContext.AddAsync(entity);
            }

            await applicationDbContext.SaveChangesAsync(CancellationToken.None);
        }

        protected static async Task<TEntity> FindAsync<TEntity>(
            int id,
            params Expression<Func<TEntity, object>>[] paths)
            where TEntity : BaseEntity, IAggregateRoot
        {
            using var scope = _scopeFactory.CreateScope();

            var applicationDbContext = GetApplicationDbContext(scope);

            var dbSet = applicationDbContext.Set<TEntity>().AsQueryable();

            foreach (var path in paths)
            {
                dbSet = dbSet.Include(path);
            }

            var entity = await dbSet.FirstAsync(e => e.Id == id);

            return entity;
        }

        private static ApplicationDbContext GetApplicationDbContext(IServiceScope scope)
        {
            var context = scope.ServiceProvider.GetService<IApplicationDbContext>();

            if (context is null)
            {
                throw new Exception($"{nameof(IApplicationDbContext)} is null");
            }

            var applicationDbContext = (ApplicationDbContext) context;

            return applicationDbContext;
        }

        protected static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetService<ISender>();

            if (mediator is null)
            {
                throw new Exception($"{nameof(ISender)} is null");
            }

            return await mediator.Send(request);
        }
    }
}