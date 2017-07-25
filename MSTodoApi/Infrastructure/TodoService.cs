using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MSTodoApi.Infrastructure.Http;
using MSTodoApi.Model;
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

        public async Task<TodosViewModel> GetTodos(DateTime dueDateTime, bool includeOverdueTasks = false, 
            string taskFields = "", string eventFields = "", bool includeCancelledEvents = false)
        {
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

                return new TodosViewModel
                {
                    Events = eventsTask.Result.Value,
                    Tasks = tasksTask.Result.Value
                };
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while getting tasks and events",e);
            }

            return new TodosViewModel();
        }
    }
}