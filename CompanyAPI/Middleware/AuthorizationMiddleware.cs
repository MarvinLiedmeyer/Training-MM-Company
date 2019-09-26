using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace CompanyAPI.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RepoExceptionMiddleware> _logger;

        public AuthorizationMiddleware(RequestDelegate next, ILoggerFactory logger)
        {
            _next = next;
            _logger = logger.CreateLogger<RepoExceptionMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            int temp = Helper.Authorization.GetUser(context).LocationID;
            var authtokenheader = context.Request.Headers["Authorization"];
            if (authtokenheader.Count == 0)
            {
                context.Response.StatusCode = 401;
                context.Response.WriteAsync("Unauthorized");
            }
            else
            {
                var client = new RestClient();
                var request = new RestRequest($"https://chaynssvc.tobit.com/v0.5/{temp}/User");
                request.AddHeader("Authorization", authtokenheader);
                var cancellationTokenSource = new CancellationTokenSource();
                var restResponse = await client.ExecuteTaskAsync(request, cancellationTokenSource.Token);
                var myObject = JsonConvert.DeserializeObject<JObject>(restResponse.Content)["data"]["uacGroups"].Children();
                var authorized = myObject.ToList<JToken>().Find(e => e["id"].ToObject<int>() == 1) != null;
                if (authorized)
                {
                    await _next(context);
                }
                else
                {
                    context.Response.StatusCode = 401;
                    context.Response.WriteAsync("Unauthorized");
                }
            }
        }
    }
}