using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Usadosbr.Contas.Core.Common.Services;
using Usadosbr.Contas.Core.Persistance;

namespace Usadosbr.Contas.Migrations
{
    public class Program
    {
        private static readonly IConfiguration? Configuration = GetConfiguration();

        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            try
            {
                var host = CreateHostBuilder(args).Build();

                using var scope = host.Services.CreateScope();

                using (var context = scope.ServiceProvider.GetRequiredService<UsadosbrAuthContext>())
                {
                    context.Database.Migrate();
                }
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Erro ao aplicar as migrações: {Message}", e.Message);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }

            Log.Information("Migrações pendendes aplicadas");
            return 0;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices(builder =>
                {
                    builder.AddSingleton<IDateTimeService, DateTimeService>();
                    builder.AddDbContext<UsadosbrAuthContext>(options =>
                    {
                        options.EnableSensitiveDataLogging();
                        options.UseSqlServer(Configuration.GetConnectionString("SqlServer"),
                            x => { x.MigrationsAssembly(typeof(Program).Assembly.FullName); });
                    });
                });

        private static IConfigurationRoot? GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
