using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace MSTodoApi.Infrastructure.Auth
{
    public class TokenProvider : ITokenProvider
    {
        private string _token = string.Empty;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetToken()
        {
            if (!string.IsNullOrEmpty(_token)) return _token;
            
            StringValues accessToken;
                  if (_httpContextAccessor.HttpContext.Request.Headers
                      .TryGetValue(Constants.TokenHeaderKey, out accessToken))
                           {
                               _token=accessToken.First();
                               return _token;
             }
            
            throw new Exception("There is no user token provided.");
        }
    }
}