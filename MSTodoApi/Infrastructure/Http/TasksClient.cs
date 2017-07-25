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
    public class TasksClient : ITasksClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<TasksClient> _logger;
        private readonly IDatetimeUtils _datetimeUtils;
        private readonly ITokenStore _tokenStore;

        private static readonly string DueTodayOrBeforeFilter
            = "$filter=DueDateTime/Datetime lt '{0}'";
        private static readonly string DueTodayFilter
            = "$filter=DueDateTime/Datetime gt '{0}' and DueDateTime/Datetime lt '{1}'";
        
        private static readonly string TasksPath = "me/tasks";

        public TasksClient(IHttpClientFactory httpClientFactory,
            ILogger<TasksClient> logger, IDatetimeUtils datetimeUtils, ITokenStore tokenStore)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _datetimeUtils = datetimeUtils;
            _tokenStore = tokenStore;
        }

        public async Task<ResponseModel<TaskModel>> GetTasks(DateTime dueDatetime, 
            bool includeOverdues = false, string fields="")
        {
            string tasksFilter = GetTasksFilter(dueDatetime, includeOverdues);
            var selectQuery = string.IsNullOrEmpty(fields) ? $"&$Select={fields}" : string.Empty;
                    
            var requestUri = $"{Constants.OutlookBaseAddress}{TasksPath}?{tasksFilter}{selectQuery}";


            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
                requestUri))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("bearer", _tokenStore.AccessToken);
                using (HttpResponseMessage response =
                    await _httpClientFactory.GetClient().SendAsync(request).ConfigureAwait(false))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning("An error occurred while fetching tasks.");
                    }

                    using (HttpContent httpContent = response.Content)
                    {
                        string content = await httpContent.ReadAsStringAsync().ConfigureAwait(false);
                        return JsonConvert.DeserializeObject<ResponseModel<TaskModel>>(content);
                    }
                }
            }
        }

        private string GetTasksFilter(DateTime dueDateTime, bool includeOverdue)
        {
            if (includeOverdue)
            {
                return string.Format(DueTodayOrBeforeFilter,
                    _datetimeUtils.FormatLongUtc(_datetimeUtils.GetEndOfDay(dueDateTime)));
            }

            return string.Format(DueTodayFilter,
                _datetimeUtils.FormatLongUtc(_datetimeUtils.GetStartOfDay(dueDateTime)),
                _datetimeUtils.FormatLongUtc(_datetimeUtils.GetEndOfDay(dueDateTime)));
        }
    }
}