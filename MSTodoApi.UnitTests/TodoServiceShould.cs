using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Moq;
using MSTodoApi.Infrastructure;
using MSTodoApi.Infrastructure.Auth;
using MSTodoApi.Infrastructure.Http;
using Xunit;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace MSTodoApi.UnitTests
{
    public class TodoServiceShould :IDisposable
    {
        readonly Mock<IEventsClient> _eventsClientMock;
        readonly Mock<ITasksClient> _tasksClientMock;
        readonly Mock<ILogger<TodoService>> _loggerMock;

        public TodoServiceShould()
        {
           _eventsClientMock = new Mock<IEventsClient>(MockBehavior.Strict);
           _tasksClientMock = new Mock<ITasksClient>(MockBehavior.Strict);
            _loggerMock = new Mock<ILogger<TodoService>>(MockBehavior.Strict);
        }
        
        [Fact]
        public async Task ReturnTasksAndEvents_WithOverdueTasks()
        {
            var dueDateTime = DateTime.Today;
            bool includeOverdueTasks = true;

            _eventsClientMock.Setup(x => x.GetEvents(dueDateTime, It.IsAny<string>(),false))
                .Returns(Task.FromResult(TestDataHelper.Events));

            _tasksClientMock.Setup(x => x.GetTasks(dueDateTime, includeOverdueTasks, It.IsAny<string>()))
                .Returns(Task.FromResult(TestDataHelper.Tasks));
            
            ITodoService service = new TodoService(_eventsClientMock.Object, _tasksClientMock.Object,_loggerMock.Object);

            var result = await service.GetTodos(
                dueDateTime: dueDateTime,
                includeOverdueTasks: includeOverdueTasks,
                taskFields: Constants.SelectedTaskFields,
                eventFields: Constants.SelectedEventFields);

            Assert.Equal(TestDataHelper.Tasks.Value.First().Subject, result.Value.Tasks.First().Subject);
            Assert.Equal(TestDataHelper.Events.Value.First().Subject, result.Value.Events.First().Subject);
        }

        [Fact]
        public async Task ReturnTasksAndEvents_WithoutOverdueTasks()
        {
            var dueDateTime = DateTime.Today;
            bool includeOverdueTasks = false;
            
            _eventsClientMock.Setup(x => x.GetEvents(dueDateTime, It.IsAny<string>(),false))
                .Returns(Task.FromResult(TestDataHelper.Events));

            _tasksClientMock.Setup(x => x.GetTasks(dueDateTime, includeOverdueTasks, It.IsAny<string>()))
                .Returns(Task.FromResult(TestDataHelper.Tasks));

            ITodoService service = new TodoService(_eventsClientMock.Object, _tasksClientMock.Object, _loggerMock.Object);

            var result = await service.GetTodos(
                dueDateTime: dueDateTime,
                includeOverdueTasks: includeOverdueTasks,
                taskFields: Constants.SelectedTaskFields,
                eventFields: Constants.SelectedEventFields);

            Assert.Equal(TestDataHelper.Tasks.Value.First().Subject, result.Value.Tasks.First().Subject);
            Assert.Equal(TestDataHelper.Events.Value.First().Subject, result.Value.Events.First().Subject);
        }

        public void Dispose()
        {
            _eventsClientMock.VerifyAll();
            _tasksClientMock.VerifyAll();
            _loggerMock.VerifyAll();
        }
    }
}