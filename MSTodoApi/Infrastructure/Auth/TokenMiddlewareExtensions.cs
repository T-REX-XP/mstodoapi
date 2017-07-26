using Microsoft.AspNetCore.Builder;

namespace MSTodoApi.Infrastructure.Auth
{
    public static class TokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenMiddleware>();
        }
    }
}