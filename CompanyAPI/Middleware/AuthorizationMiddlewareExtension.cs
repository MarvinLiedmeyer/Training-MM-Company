using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace CompanyAPI.Middleware
{
    public static class AuthorizationMiddlewareExtension
    {
        public static void UseAuthorizationMiddlewareExtension(this IApplicationBuilder app)
        {
            app.UseMiddleware<AuthorizationMiddleware>();
        }
    }
}
