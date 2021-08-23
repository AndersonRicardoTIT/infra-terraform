using System;
using System.Data;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Usadosbr.Contas.Core.Common;
using Usadosbr.Contas.Core.Common.Pipelines;
using Usadosbr.Contas.Core.Common.Services;
using Usadosbr.Contas.Core.Entities;
using Usadosbr.Contas.Core.Features.Usuarios.Commands.CriaUsuarioCommand;
using Usadosbr.Contas.Core.Persistance;
using Usadosbr.Contas.WebApi.Shared;

namespace Usadosbr.Contas.WebApi
{
    public static class CoreDependencyInjector
    {
        public static void AddCoreDependencies(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IDateTimeService, DateTimeService>();

            services.AddDbContext<UsadosbrAuthContext>(options =>
            {
                options.UseSqlServer(connectionString, x => x.MigrationsAssembly("Usadosbr.Contas.Migrations"));
            });

            services.AddDbContext<UsadosbrAuthContext>(options =>
            {
                options.UseSqlServer(connectionString, x => x.MigrationsAssembly("Usadosbr.Contas.Migrations"));
            });

            services.AddIdentity<UsadosbrUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<UsadosbrAuthContext>()
                .AddErrorDescriber<PortugueseIdentityErrorDescriber>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequireDigit = PasswordConfiguration.RequireDigit;
                options.Password.RequireLowercase = PasswordConfiguration.RequireLowercase;
                options.Password.RequireUppercase = PasswordConfiguration.RequireUppercase;
                options.Password.RequireNonAlphanumeric = PasswordConfiguration.RequireNonAlphanumeric;
                options.Password.RequiredLength = PasswordConfiguration.RequiredLength;
                options.Password.RequiredUniqueChars = PasswordConfiguration.RequiredUniqueChars;
            });
            
            services.AddMediatR(typeof(UsadosbrAuthContext));

            services.AddValidatorsFromAssemblyContaining<CriaUsuarioCommandValidator>();

            services.AddScoped(typeof(IRequestPreProcessor<>), typeof(QueryValidationBehaviour<>));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));

            services.AddTransient<IDbConnection, SqlConnection>(_ => new SqlConnection(connectionString));
        }
    }
}