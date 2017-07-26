using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MSTodoApi.IntegrationTests.Infrastructure
{
    public class RefreshTokenHandler : DelegatingHandler
    {
        public static string TokenUrl = "https://login.microsoftonline.com/common/oauth2/v2.0/token";

        private readonly string _scopes = "openid" +
                                          "+offline_access" +
                                          "+profile" +
                                          "+https%3A%2F%2Foutlook.office.com%2Fcalendars.readwrite" +
                                          "+https%3A%2F%2Foutlook.office.com%2Fcalendars.readwrite.shared" +
                                          "+https%3A%2F%2Foutlook.office.com%2Ftasks.readwrite" +
                                          "+https%3A%2F%2Foutlook.office.com%2Ftasks.readwrite.shared" +
                                          "+https%3A%2F%2Foutlook.office.com%2Fuser.readbasic.all";

        public  ITokenStore TokenStore { get; set; }
        public  AppCredentials Credentials { get; set; }
      
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
                new KeyValuePair<string, string>("scope", _scopes),
                new KeyValuePair<string, string>("client_id", Credentials.AppId),
                new KeyValuePair<string, string>("client_secret", Credentials.AppSecret)
            };

            using (HttpContent content = new FormUrlEncodedContent(postData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                using (HttpResponseMessage tokenResponse = await client.PostAsync(TokenUrl, content))
                {
                    TokenStore.UpdateTokens(await tokenResponse.Content.ReadAsStringAsync());
                }
            }
        }
    }
}