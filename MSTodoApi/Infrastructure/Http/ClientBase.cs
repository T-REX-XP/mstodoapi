using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MSTodoApi.Infrastructure.Auth;
using MSTodoApi.Model;
using Newtonsoft.Json;

namespace MSTodoApi.Infrastructure.Http
{
    public class ClientBase
    {
        private readonly ITokenStore _tokenStore;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ClientBase> _logger;

        public ClientBase(ITokenStore tokenStore, 
            IHttpClientFactory httpClientFactory, 
            ILogger<ClientBase> logger)
        {
            _tokenStore = tokenStore;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        protected async Task<ResponseModel<T>> Request<T>(HttpMethod httpMethod, string requestUri)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(httpMethod,
                requestUri))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("bearer", _tokenStore.AccessToken);
                
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
                        return JsonConvert.DeserializeObject<ResponseModel<T>>(content);
                    }
                }
            }
        }
    }
}