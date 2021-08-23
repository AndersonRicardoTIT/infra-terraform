using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Usadosbr.Contas.WebApi.Shared;
using Usadosbr.Contas.WebApi.Shared.Result;

namespace Usadosbr.Contas.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private string? ConnectionString => Configuration.GetConnectionString("SqlServer");

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ResultActionFilter>();

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ResultActionFilter));
            });
            
            services.AddCoreDependencies(ConnectionString!);

            services.AddApiVersions();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            app.UseSerilogRequestLogging();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(
                    options =>
                    {
                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            options.SwaggerEndpoint(
                                $"/swagger/{description.GroupName}/swagger.json",
                                description.GroupName.ToUpperInvariant());
                        }
                    });
            }
            
            if (env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}