using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MSTodoApi.Infrastructure.Auth
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }
 
        public async Task Invoke(HttpContext httpContext)
        {
    
            if (!httpContext.Request.Headers.Keys.Contains(Constants.TokenHeaderKey))
            {
                await BadRequest(httpContext);
                return;
            }
            
            
            await _next.Invoke(httpContext);
        }

        private static async Task BadRequest(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = 400; //Bad Request                
            await httpContext.Response.WriteAsync("MS Outlook API Token is missing");
        }
    }
}