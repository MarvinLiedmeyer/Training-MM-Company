using System.Threading.Tasks;
using Chayns.Auth.ApiExtensions;
using Microsoft.AspNetCore.Http;

namespace CompanyAPI.Provider
{
    public class TokenRequirementProvider : TokenRequirementProviderBase
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenRequirementProvider(IHttpContextAccessor contextAccessor)
        {
            // IHttpContextAccessor will be injected by dependency injection
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Returns the locationId from current url path
        /// </summary>
        /// <returns>The locationId that is required</returns>
        public override Task<int?> GetRequiredLocationId()
        {
            // Get parts of current relative path
            var pathParts = _contextAccessor.HttpContext.Request.Path.Value.Trim('/').Split('/');

            // Schema of api is /api/{locationId}/[controller]
            var locationIdString = pathParts[1];

            return int.TryParse(locationIdString, out var locationId) ? Task.FromResult((int?)locationId) : Task.FromResult(default(int?));
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <returns></returns>
        public override Task<string> GetRequiredSiteId() => Task.FromResult(default(string));
    }
}
