using Catalog.Application.Handlers;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Catalog.API
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {

            // MediatR.Extensions.Microsoft.DependencyInjection (deprecated -> recommended MediatR)
            // Microsoft.Extensions.DependencyInjection
            // Microsoft.Extensions.DependencyInjection.Abstractions
            // Microsoft.Extensions.Logging.Abstractions


            services.AddControllers();

            //Package: Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
            services.AddApiVersioning();  

            //Package: AspNetCore.HealthChecks.UI.Client
            services.AddHealthChecks()    
                .AddMongoDb(configuration["DatabaseSettings:ConnectionString"], "Catalog MongoDB Health Check", HealthStatus.Degraded);

            //Package: Swashbuckle.AspNetCore
            services.AddSwaggerGen(c => c.SwaggerDoc("v1",new OpenApiInfo(){ Title = "Catalog.API", Version="v1"}));

            //DI

            //Package: Automapper.Extensions.Microsoft.DependencyInjection
            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(typeof(CreateProductHandler).GetTypeInfo().Assembly);
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddScoped<ICatalogContext,CatalogContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBrandRepository, ProductRepository>();
            services.AddScoped<ITypesRepository, ProductRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1"));
            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
