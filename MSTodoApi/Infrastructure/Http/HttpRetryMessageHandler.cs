using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Polly;

namespace MSTodoApi.Infrastructure.Http
{
    public class HttpRetryMessageHandler : DelegatingHandler
    {
        public  IOptions<AppRetryOptions> Options { get; set; }
        
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpStatusCode[] httpStatusCodesWorthRetrying = {
                HttpStatusCode.RequestTimeout, // 408
                HttpStatusCode.InternalServerError, // 500
                HttpStatusCode.BadGateway, // 502
                HttpStatusCode.ServiceUnavailable, // 503
                HttpStatusCode.GatewayTimeout // 504
            };
            
            var httpResponseMessage = await Policy
                .Handle<HttpRequestException>()    
                .Or<TaskCanceledException>()
                .OrResult<HttpResponseMessage>(r => httpStatusCodesWorthRetrying.Contains(r.StatusCode))
                .WaitAndRetryAsync(Options.Value.HttpReqRetryCount,
                    retryCount => TimeSpan.FromSeconds(Math.Pow(3, retryCount)))
                .ExecuteAsync(async () => await base.SendAsync(request, cancellationToken));

            return httpResponseMessage;
        }
    }
}
