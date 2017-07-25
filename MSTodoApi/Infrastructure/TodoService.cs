using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MSTodoApi.Infrastructure.Http;
using MSTodoApi.ViewModel;

namespace MSTodoApi.Infrastructure
{
    public class TodoService : ITodoService
    {
        private readonly IEventsClient _eventsClient;
        private readonly ITasksClient _tasksClient;
        private readonly ILogger<TodoService> _logger;

        public TodoService(IEventsClient eventsClient, ITasksClient tasksClient, ILogger<TodoService> logger)
        {
            _eventsClient = eventsClient;
            _tasksClient = tasksClient;
            _logger = logger;
        }

        public async Task<OperationResult<TodosViewModel>> GetTodos(DateTime dueDateTime, bool includeOverdueTasks = false, 
            string taskFields = "", string eventFields = "", bool includeCancelledEvents = false)
        {
            var result = new OperationResult<TodosViewModel>();
            
            try
            {
                var eventsTask = _eventsClient.GetEvents(
                    dueDateTime:dueDateTime, 
                    fields:eventFields,
                    includeCancelledEvents:false);
                var tasksTask = _tasksClient.GetTasks(
                    dueDatetime: dueDateTime, 
                    includeOverdues: includeOverdueTasks, 
                    fields: taskFields);

                await Task.WhenAll(eventsTask, tasksTask);

                result.Value = new TodosViewModel
                {
                    Events = eventsTask.Result.Value,
                    Tasks = tasksTask.Result.Value
                };
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while getting tasks and events",e);
                result.Value = null;
            }

            return result;
        }
    }
}