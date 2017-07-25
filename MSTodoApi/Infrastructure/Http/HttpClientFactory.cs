using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using MSTodoApi.Infrastructure.Auth;

namespace MSTodoApi.Infrastructure.Http
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly ITokenStore _tokenStore;
        private readonly IOptions<AppRetryOptions> _appRetryOptions;
        private readonly IOptions<AppAuthOptions> _appAuthOptions;

        public HttpClientFactory(ITokenStore tokenStore, 
            IOptions<AppAuthOptions> appAuthOptions,
            IOptions<AppRetryOptions> appRetryOptions)
        {
            _tokenStore = tokenStore;
            _appAuthOptions = appAuthOptions;
            _appRetryOptions = appRetryOptions;
        }
        public HttpClient GetClient()
        {
            //chain: httpclient -> retry_handler -> refreshtoken_handler -> httpclient_handler
            var refreshTokenHandler = new RefreshTokenHandler{
                TokenStore = _tokenStore, 
                Options = _appAuthOptions, 
                InnerHandler = new HttpClientHandler()
            };
            
            HttpRetryMessageHandler retryMessageHandler = new HttpRetryMessageHandler
            {
                Options = _appRetryOptions,
                InnerHandler = refreshTokenHandler
            };

            var client = _appRetryOptions.Value.HandlerEnabled 
                ? new HttpClient(retryMessageHandler) 
                : new HttpClient(refreshTokenHandler);
         

            SetDefaults(client);

            return client;
        }

        private void SetDefaults(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}