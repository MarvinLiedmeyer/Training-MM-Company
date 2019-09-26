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
