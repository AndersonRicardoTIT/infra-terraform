using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Respawn;
using Usadosbr.Contas.Core.Persistance;

namespace Usadosbr.Contas.IntegrationTests.Shared
{
    public class WebApiFactory : WebApplicationFactory<WebApi.Startup>
    {
        public string? ConnectionString;
        
        public IServiceScopeFactory? ScopeFactory;
        
        private readonly Checkpoint _checkpoint;

        public WebApiFactory()
        {
            _checkpoint = new Checkpoint
            {
                TablesToIgnore = new[] { "__EFMigrationsHistory" }
            };
        }

        // See https://github.com/dotnet-architecture/eShopOnWeb/issues/465
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = builder.Build();

            var services = host.Services;

            ScopeFactory = services.GetRequiredService<IServiceScopeFactory>();

            EnsureDatabase(services);

            host.Start();

            return host;
        }

        private void EnsureDatabase(IServiceProvider? services)
        {
            using var scope = services!.CreateScope();
            
            var serviceProvider = scope.ServiceProvider;

            var context = serviceProvider.GetRequiredService<UsadosbrAuthContext>();

            if (context.Database.IsRelational())
            {
                ConnectionString = context.Database.GetConnectionString()!;

                context.Database.Migrate();
            }
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("IntegrationTest");
        }

        public async Task ResetDatabase()
        {
            if (ConnectionString != null)
            {
                await _checkpoint.Reset(ConnectionString);
            }
        }

        public async Task<T?> FirstOrDefault<T>(Expression<Func<T, bool>> expr)
            where T : class
        {
            if (ScopeFactory == null)
            {
                throw new InvalidOperationException("O service provider não foi inicializado corretamente");
            }

            using var scope = ScopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<UsadosbrAuthContext>();

            var entry = context.Set<T>();

            return await entry.FirstOrDefaultAsync(expr);
        }

        public int Add<T>(T entity)
            where T : class
        {
            if (ScopeFactory == null)
            {
                throw new InvalidOperationException("O service provider não foi inicializado corretamente");
            }

            using var scope = ScopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<UsadosbrAuthContext>();

            var entry = context.Set<T>();

            entry.Add(entity);

            return context.SaveChanges();
        }
    }
}