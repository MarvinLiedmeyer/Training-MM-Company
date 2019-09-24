using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace CompanyAPI.Middleware
{
    public static class RepoExceptionMiddlewareExtensions
    {
        public static void UseRepoExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<RepoExceptionMiddleware>();
        }
    }
}
