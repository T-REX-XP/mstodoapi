using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MSTodoApi.Infrastructure.Auth;
using MSTodoApi.Infrastructure.Utils;
using MSTodoApi.Model;

namespace MSTodoApi.Infrastructure.Http
{
    public class TasksClient : ClientBase, ITasksClient
    {
        private readonly IDatetimeUtils _datetimeUtils;

        private static readonly string DueTodayOrBeforeFilter
            = "$filter=DueDateTime/Datetime lt '{0}'";
        private static readonly string DueTodayFilter
            = "$filter=DueDateTime/Datetime gt '{0}' and DueDateTime/Datetime lt '{1}'";
        
        private static readonly string TasksPath = "me/tasks";

        public TasksClient(IHttpClientFactory httpClientFactory,
            ILogger<TasksClient> logger, IDatetimeUtils datetimeUtils, ITokenStore tokenStore)
            :base(tokenStore,httpClientFactory,logger)
        {
            _datetimeUtils = datetimeUtils;
        }

        public async Task<ResponseModel<TaskModel>> GetTasks(DateTime dueDatetime, 
            bool includeOverdues = false, string fields="")
        {
            string tasksFilter = GetTasksFilter(dueDatetime, includeOverdues);
            var selectQuery = string.IsNullOrEmpty(fields) ? $"&$Select={fields}" : string.Empty;
                    
            var requestUri = $"{Constants.OutlookBaseAddress}{TasksPath}?{tasksFilter}{selectQuery}";

            return await Request<TaskModel>(HttpMethod.Get,requestUri);

        }

        private string GetTasksFilter(DateTime dueDateTime, bool includeOverdues)
        {
            if (includeOverdues)
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