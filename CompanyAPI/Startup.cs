﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chayns.Auth.ApiExtensions;
using CompanyAPI.Helper;
using CompanyAPI.Interface;
using CompanyAPI.Middleware;
using CompanyAPI.Model;
using CompanyAPI.Repository;
using ConsoleApp.Model;
using ConsoleApp.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CompanyAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<DbSettings>(Configuration.GetSection("DbSettings"));
            services.AddChaynsAuth();

            services.AddScoped<IBaseInterface<CompanyDto, Company>, CompanyRepository>();
            services.AddScoped<IBaseInterface<DepartmentDto, Department>, DepartmentRepository>();
            services.AddScoped<IBaseInterface<EmployeeDto, Employee>, EmployeeRepository>();
            services.AddScoped<IBaseInterface<AddressDto, Address>, AddressRepository>();

            services.AddSingleton<IDbContext, DbContext>();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.InitChaynsAuth();
            app.UseHttpsRedirection();
            app.UseRepoExceptionMiddleware();
            app.UseMvc();
        }
    }
}
