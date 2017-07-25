using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace MSTodoApi.Infrastructure.Auth
{
    public class RefreshTokenHandler : DelegatingHandler
    {
        public  ITokenStore TokenStore { get; set; }
        public  IOptions<AppAuthOptions> Options { get; set; }
      
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
          
            var httpResponseMessage = await base.SendAsync(request, cancellationToken);
            
            if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                using (var client = new HttpClient())
                {
                    await RefreshToken(client);
                    
                    //Replay request (NOTE: This logic can be improved with request queueing mechanism)
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenStore.AccessToken);
                    httpResponseMessage = await base.SendAsync(request, cancellationToken);
                }
            }

            return httpResponseMessage;
        }

        private async Task RefreshToken(HttpClient client)
        {
            var postData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("refresh_token", TokenStore.RefreshToken),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("client_id", Options.Value.AppId),
                new KeyValuePair<string, string>("client_secret", Options.Value.AppSecret)
            };

            using (HttpContent content = new FormUrlEncodedContent(postData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                using (HttpResponseMessage tokenResponse = await client.PostAsync(Constants.TokenUrl, content))
                {
                    TokenStore.UpdateTokens(await tokenResponse.Content.ReadAsStringAsync());
                }
            }
        }
    }
}