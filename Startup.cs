﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using asp.net_core_trip_manager.Persistence;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using asp.net_core_trip_manager.Core.Services;
using asp.net_core_trip_manager.Persistence.Services;
using asp.net_core_trip_manager.Core.Models;
using asp.net_core_trip_manager.Core.Dtos;
using asp.net_core_trip_manager.Core.Repositories;
using asp.net_core_trip_manager.Persistence.Repositories;

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

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
                config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
                // Set of callbacks while the authentication is happening
                config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = async ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                        {
                            ctx.Response.StatusCode = 401;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                        await Task.Yield();
                    }
                };
            })
            .AddEntityFrameworkStores<TripContext>();

            services.AddDbContext<TripContext>();

            // Create one per request cycle
            services.AddScoped<ITripRepository, TripRepository>();

            services.AddScoped<IStopRepository, StopRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // When we need it create a new copy
            services.AddTransient<IGeoCoordsService, GMapsGeoCoordsService>();

            // Create this every time we need it
            services.AddTransient<TripContextSeedData>();

            services.AddLogging();
            
            services.AddMvc(config =>
            {
                if (_env.IsProduction())
                {
                    config.Filters.Add(new RequireHttpsAttribute());
                }
            })
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

            app.UseIdentity();

            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller=App}/{action=Index}/{id?}");
            });

            seeder.EnsureSeedData().Wait();
        }
    }
}
