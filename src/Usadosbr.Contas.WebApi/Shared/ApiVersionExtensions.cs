using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Usadosbr.Contas.WebApi.Shared.Constants;

namespace Usadosbr.Contas.WebApi.Shared
{
    public static class ApiVersionExtensions
    {
        public static void AddApiVersions(this IServiceCollection services)
        {
            services.AddApiVersioning(
                options =>
                {
                    options.DefaultApiVersion = ApiVersions.V1ApiVersion;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;
                });

            services.AddVersionedApiExplorer(options =>
            {
                options.DefaultApiVersion = ApiVersions.V1ApiVersion;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.GroupNameFormat = "'v'VVV";
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(
                options =>
                {
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                    options.IncludeXmlComments(xmlPath);
                });
        }
    }
}