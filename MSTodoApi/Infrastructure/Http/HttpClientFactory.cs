using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace MSTodoApi.Infrastructure.Http
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly IOptions<AppRetryOptions> _appRetryOptions;

        public HttpClientFactory(IOptions<AppRetryOptions> appRetryOptions)
        {
            _appRetryOptions = appRetryOptions;
        }
        public HttpClient GetClient()
        {
            //chain: httpclient -> (conditional)[retry_handler] -> httpclient_handler

            var httpMessageHandler = new HttpClientHandler();
            
            HttpRetryMessageHandler retryMessageHandler = new HttpRetryMessageHandler
            {
                Options = _appRetryOptions,
                InnerHandler = httpMessageHandler
            };

            var client = _appRetryOptions.Value.HandlerEnabled 
                ? new HttpClient(retryMessageHandler) 
                : new HttpClient(httpMessageHandler);
         

            SetDefaults(client);

            return client;
        }

        private void SetDefaults(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}