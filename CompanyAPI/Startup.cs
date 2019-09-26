using Chayns.Auth.ApiExtensions;
using Chayns.Auth.Shared.Constants;
using CompanyAPI.Helper;
using CompanyAPI.Interface;
using CompanyAPI.Middleware;
using CompanyAPI.Model;
using CompanyAPI.Provider;
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
            // Add authentication filter for only manager of the site {77893-11922} ----------------------------
            services.AddChaynsAuth(typeof(TokenRequirementProvider));
            services.AddMvc();
            // -------------------------------------------------------------------------------------------------

            // Add DbSettings service which take the settings ------------------------
            services.Configure<DbSettings>(Configuration.GetSection("DbSettings"));
            // -----------------------------------------------------------------------

            // Add required middlewares for authentication
            services.AddChaynsAuth();
            // -------------------------------------------

            // Add service scopes. Scoped services are generally created per web request ----------
            services.AddScoped<IBaseInterface<CompanyDto, Company>, CompanyRepository>();
            services.AddScoped<IBaseInterface<DepartmentDto, Department>, DepartmentRepository>();
            services.AddScoped<IBaseInterface<EmployeeDto, Employee>, EmployeeRepository>();
            services.AddScoped<IBaseInterface<AddressDto, Address>, AddressRepository>();
            // ------------------------------------------------------------------------------------

            // Add service singelton. They are created only one time per application and used for whole the life time
            // Takes the Api-Settings for the context ---------------------------------------------------------------
            services.AddSingleton<IDbContext, DbContext>();
            // ------------------------------------------------------------------------------------------------------
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

            // Add middlewares for Auth
            app.InitChaynsAuth();
            // ------------------------

            // HTTPS-Umleitung middleware zum Umleiten von HTTP-Anforderungen an HTTPS
            app.UseHttpsRedirection();
            // -----------------------------------------------------------------------

            // Add middlewares for Exception --
            app.UseRepoExceptionMiddleware();
            // --------------------------------

            // UseMvc is an extension on the IApplicationBuilder which takes an Action delegate of IRouteBuilder. The IRouteBuilder will be used to configure the routing for MVC.
            app.UseMvc();
            // -------------------------------------------------------------------------------------------------------------------------------------------------------------------
        }
    }
}
