using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MSTodoApi.Infrastructure.Auth;
using Newtonsoft.Json;

namespace MSTodoApi.Infrastructure.Http
{
    public class ClientBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ClientBase> _logger;
        private readonly ITokenProvider _tokenProvider;

        public ClientBase(IHttpClientFactory httpClientFactory, ILogger<ClientBase> logger, 
            ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _tokenProvider = tokenProvider;
        }
        protected async Task<T> Request<T>(HttpMethod httpMethod, string requestUri)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(httpMethod,
                requestUri))
            {
                var accessToken = _tokenProvider.GetToken();
                request.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken );
                
                using (HttpResponseMessage response =
                    await _httpClientFactory.GetClient().SendAsync(request).ConfigureAwait(false))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning($"An error occurred while fetching, req: {requestUri}");
                    }

                    using (HttpContent httpContent = response.Content)
                    {
                        string content = await httpContent.ReadAsStringAsync().ConfigureAwait(false);
                        return JsonConvert.DeserializeObject<T>(content);
                    }
                }
            }
        }
    }
}