using Chayns.Auth.ApiExtensions;
using Chayns.Auth.Shared.Constants;
using CompanyAPI.Helper;
using CompanyAPI.Interface;
using CompanyAPI.Middleware;
using CompanyAPI.Model;
using CompanyAPI.Repository;
using ConsoleApp.Model;
using ConsoleApp.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new ChaynsAuthAttribute(true, uac: Uac.Manager, uacSiteId: "77893-11922"));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<DbSettings>(Configuration.GetSection("DbSettings"));
            services.AddChaynsAuth();
            services.AddScoped<IBaseInterface<CompanyDto, Company>, CompanyRepository>();
            services.AddScoped<IBaseInterface<DepartmentDto, Department>, DepartmentRepository>();
            services.AddScoped<IBaseInterface<EmployeeDto, Employee>, EmployeeRepository>();
            services.AddScoped<IBaseInterface<AddressDto, Address>, AddressRepository>();
            services.AddSingleton<IDbContext, DbContext>();
        }
        
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
