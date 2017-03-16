using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using asp.net_core_trip_manager.Services;
using Microsoft.Extensions.Configuration;
using asp.net_core_trip_manager.Persistence;
using asp.net_core_trip_manager.Models;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using asp.net_core_trip_manager.Dtos;

namespace asp.net_core_trip_manager
{
    public class Startup
    {
        private IHostingEnvironment _env;
        private IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath) // Root of project
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            _config = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Need single instance that we can share across
            services.AddSingleton(_config);

            if (_env.IsEnvironment("Development") || _env.IsEnvironment("Testing"))
            {
                // DebugMailService reused only in scope of single request
                services.AddScoped<IMailService, DebugMailService>();
            }
            else
            {
                // Implement Real Service
            }

            services.AddDbContext<TripContext>();

            // Create one per request cycle
            services.AddScoped<ITripRepository, TripRepository>();

            // When we need it create a new copy
            services.AddTransient<IGeoCoordsService, GMapsGeoCoordsService>();

            // Create this every time we need it
            services.AddTransient<TripContextSeedData>();

            services.AddLogging();
            
            services.AddMvc()
                .AddJsonOptions(config =>
                {
                    config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, TripContextSeedData seeder)
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<TripDto, Trip>().ReverseMap();
                config.CreateMap<StopDto, Stop>().ReverseMap();
            });

            if (env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddDebug(LogLevel.Information);
            }
            else
            {
                loggerFactory.AddDebug(LogLevel.Error);
            }
            
            app.UseStaticFiles();

            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "App", action = "Index" }
                    );
            });

            seeder.EnsureSeedData().Wait();
        }
    }
}
