using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MSTodoApi.Infrastructure.Auth;
using MSTodoApi.Infrastructure.Utils;
using MSTodoApi.Model;
using Newtonsoft.Json;

namespace MSTodoApi.Infrastructure.Http
{
    public class EventsClient : IEventsClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EventsClient> _logger;
        private readonly IDatetimeUtils _datetimeUtils;
        private readonly ITokenStore _tokenStore;
        private static readonly string EventsPath = "me/calendarview";
        private static readonly string DueTodayFilter = "startdatetime={0}&enddatetime={1}";
        private static readonly string IsCancelledFilter = "$filter= IsCancelled eq false";

        public EventsClient(IHttpClientFactory httpClientFactory, ILogger<EventsClient> logger, 
            IDatetimeUtils datetimeUtils, ITokenStore tokenStore)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _datetimeUtils = datetimeUtils;
            _tokenStore = tokenStore;
        }

        public async Task<ResponseModel<EventModel>> GetEvents(DateTime dueDateTime, string fields = "", 
            bool includeCancelledEvents = false)
        {
            string eventsFilter = GetEventsFilter(dueDateTime, includeCancelledEvents);
            
            var selectQuery = string.IsNullOrEmpty(fields) ? $"&$Select={fields}" : string.Empty;
            
            string requestUri = $"{Constants.OutlookBaseAddress}{EventsPath}?{eventsFilter}&{selectQuery}";
            
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("bearer", _tokenStore.AccessToken);
                using (HttpResponseMessage response = await _httpClientFactory.GetClient().SendAsync(request).ConfigureAwait(false))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning("An error occurred while fetching events.");
                    }

                    using (HttpContent httpContent = response.Content)
                    {
                        string content = await httpContent.ReadAsStringAsync().ConfigureAwait(false);

                        return JsonConvert.DeserializeObject<ResponseModel<EventModel>>(content);
                    }
                }
            }
        }

        private string GetEventsFilter(DateTime dueDatetime, bool includeCancelledEvents)
        {
            var eventsFilter = string.Format(DueTodayFilter,
                _datetimeUtils.FormatLongUtc(_datetimeUtils.GetStartOfDay(dueDatetime)),
                _datetimeUtils.FormatLongUtc(_datetimeUtils.GetEndOfDay(dueDatetime)));

            if (includeCancelledEvents == false)
            {
                eventsFilter = $"{eventsFilter}&{IsCancelledFilter}";
            }

            return eventsFilter;
        }
    }
}