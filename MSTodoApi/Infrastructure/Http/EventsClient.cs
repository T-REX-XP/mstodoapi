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
    public class EventsClient : ClientBase, IEventsClient
    {
        private readonly IDatetimeUtils _datetimeUtils;
        private static readonly string EventsPath = "me/calendarview";
        private static readonly string DueTodayFilter = "startdatetime={0}&enddatetime={1}";
        private static readonly string IsCancelledFilter = "$filter= IsCancelled eq false";

        public EventsClient(IHttpClientFactory httpClientFactory, ILogger<EventsClient> logger, 
            IDatetimeUtils datetimeUtils, ITokenStore tokenStore):
            base(tokenStore,httpClientFactory,logger)
        {
            _datetimeUtils = datetimeUtils;
        }

        public async Task<ResponseModel<EventModel>> GetEvents(DateTime dueDateTime, string fields = "", 
            bool includeCancelledEvents = false)
        {
            string eventsFilter = GetEventsFilter(dueDateTime, includeCancelledEvents);
            
            var selectQuery = string.IsNullOrEmpty(fields) ? $"&$Select={fields}" : string.Empty;
            
            string requestUri = $"{Constants.OutlookBaseAddress}{EventsPath}?{eventsFilter}&{selectQuery}";

            return await Request<EventModel>(HttpMethod.Get, requestUri);
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