﻿using System;
using System.IO;
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
    public class TestBase : IAsyncLifetime
    {
        protected readonly IFixture Fixture = TestFixture.Get();
        
        private static IConfigurationRoot _configuration;
        private static IServiceScopeFactory _scopeFactory;

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

            var context = scope.ServiceProvider.GetService<IApplicationDbContext>();

            if (context is null)
            {
                throw new Exception($"{nameof(IApplicationDbContext)} is null");
            }

            foreach (var entity in entities)
            {
                await ((ApplicationDbContext) context).AddAsync(entity);
            }

            await context.SaveChangesAsync(CancellationToken.None);
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