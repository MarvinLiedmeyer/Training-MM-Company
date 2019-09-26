using System;
using System.Net;
using System.Threading.Tasks;
using CompanyAPI.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CompanyAPI.Middleware
{
    public class RepoExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RepoExceptionMiddleware> _logger;

        public RepoExceptionMiddleware(RequestDelegate next, ILoggerFactory logger)
        {
            _next = next;
            _logger = logger.CreateLogger<RepoExceptionMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (RepoException ex)
            {
                switch (ex.ExType)
                {
                    case RepoResultType.SQLERROR:
                        _logger.LogError(ex, "ServiceUnavailable- SQL Error");
                        context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                        break;
                    case RepoResultType.NOTFOUND:
                        _logger.LogWarning(ex, "Conflict - Not Found");
                        context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                        break;
                    case RepoResultType.WRONGPARAMETER:
                        _logger.LogWarning(ex, "Bad Request - Wrong Parameter");
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    default:
                        _logger.LogWarning(ex, "Conflict");
                        context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Request failed ver heavily");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }
    }
}
