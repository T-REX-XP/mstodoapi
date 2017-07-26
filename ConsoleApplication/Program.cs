using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Identity.Client;

namespace ConsoleApplication
{
    class Program
    {
        private static string refreshToken =
                "OAQABAAAAAABnfiG-mA6NTae7CdWW7Qfd78OSeagbZzOCAPLxm4FV8-FXuccuS818yLgeEgODTWFT0leISUmHFRXpHvcg14mWNv5g-Md-ffg_SflMh_WodHyM8LTW2s07Juewu34ElrdIWSR1Yv485HrfpfumN9TFb0IK3hd-2I3GP4mXszXb6nbAAKSOWO5kJ86UMcxhc-3sROXnaB5cRFSaM4YklBatj4iGt4N-h_oXJHaJL5xThMKvd96vJqshvid6xxC_S2mD9ATqrfaY93o9ljr0JvgzgtHDrAkBENdXkTEotUPvvbAcx-uiIuqRwctrk4OtASP8-JkayKhxji2xBAZzpbyIysvrjrHOHe01r1IB7GZm1JQtqukwLpdwkUB8y9FdydRcAPLSsIoMxijBRQnp1N5k2rk8u7So1ZpNLq_vwhHXIDpjvqI5ccSXeRc5csakMcdysXfzNDWa_X-v1Rjt5MwlUzJa1gSSpD8SkdxZGGc-uwm9K9XSfwKopH_Xpa4jY00HBCyxluFSh0oU3anyMKkBxNx95ke2gYf3eMHQeMzKfURrOw4PFtmJvDf0GXO5ZtoZnA4P_1wEIMcG4kGzL62pWrgZuR7PnQ64ywWw9TYCt15BRhBNVaY2aRv_tFa_KwEx_L05fiaEi4wfoAtjl6x7dV8Q-LZcgdgcJDVFXkS_kGpBCHsOTtvsLRk_tNZq9WzXABrz_RZTmMEoPIgLp4cKle0DPVI1XcXawgfwtwBt_VggX2M0hVMgjnXC-s8VkQp9g_Aa-TuejeDOlZrcAgUzrIMID8guF0KZHARxOUV7YyAA";

        private static string _scopes = "openid" +
                                        "+offline_access" +
                                        "+profile" +
                                        "+https://outlook.office.com%2Fcalendars.readwrite" +
                                        "+https%3A%2F%2Foutlook.office.com%2Fcalendars.readwrite.shared" +
                                        "+https%3A%2F%2Foutlook.office.com%2Ftasks.readwrite" +
                                        "+https%3A%2F%2Foutlook.office.com%2Ftasks.readwrite.shared" +
                                        "+https%3A%2F%2Foutlook.office.com%2Fuser.readbasic.all";

        static List<string> scopesList = new List<string>()
        {
            "user.readbasic.all",
            "tasks.readwrite.shared","tasks.readwrite",
            "calendars.readwrite.shared","calendars.readwrite"
        };
        static void Main(string[] args)
        {
            AuthenticationResult authResult = null;
            PublicClientApplication publicClientApp = new PublicClientApplication("6368f5ee-a9fd-497d-b8a2-fc46fe624543");
            try
            {
               
                authResult =  publicClientApp.AcquireTokenSilentAsync(scopesList, publicClientApp.Users.FirstOrDefault()).Result;
            }
            catch (Exception e)
            {
                try
                {
                    authResult = publicClientApp.AcquireTokenAsync(scopesList).Result;
                }
                catch (MsalException msalex)
                {
                    Console.WriteLine(
                        $"Error Acquiring Token:{System.Environment.NewLine}{msalex}");
                }
                
                Console.WriteLine(e);
                            }
            
            using (var client = new HttpClient())
            {
                var postData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("refresh_token", refreshToken),
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                    new KeyValuePair<string, string>("scope", _scopes),
                    new KeyValuePair<string, string>("client_id", "6368f5ee-a9fd-497d-b8a2-fc46fe624543"),
                    new KeyValuePair<string, string>("client_secret", "KjMtXbTFppkgziCVLwexNmp")
                };

                using (HttpContent content = new FormUrlEncodedContent(postData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                    using (HttpResponseMessage tokenResponse =  
                        client.PostAsync("https://login.microsoftonline.com/common/oauth2/v2.0/token", content).Result)
                    {
                        string readAsStringAsync = tokenResponse.Content.ReadAsStringAsync().Result;
                       
                    }
                }
            }
        }
    }
}