using System.Net.Http;
using MSTodoApi.Infrastructure;
using MSTodoApi.Infrastructure.Http;
using RichardSzalay.MockHttp;

namespace MSTodoApi.UnitTests
{
    public class MockHttpClientFactory : IHttpClientFactory
    {
        public HttpClient GetClient()
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When($"{Constants.OutlookBaseAddress}me/tasks")
                .Respond("application/json", TestDataHelper.TasksJson);

            mockHttp.When($"{Constants.OutlookBaseAddress}me/calendarview*")
                .Respond("application/json", TestDataHelper.EventsJson);

            var client = mockHttp.ToHttpClient();

            return client;
        }
    }
}