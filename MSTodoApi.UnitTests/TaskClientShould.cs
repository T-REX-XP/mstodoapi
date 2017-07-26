using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using MSTodoApi.Infrastructure;
using MSTodoApi.Infrastructure.Auth;
using MSTodoApi.Infrastructure.Http;
using MSTodoApi.Infrastructure.Utils;
using MSTodoApi.Model;
using Xunit;

namespace MSTodoApi.UnitTests
{
    public class TaskClientShould :IDisposable
    {
        private readonly Mock<IDatetimeUtils> _dateTimeUtilsMock;
        private readonly Mock<ILogger<TasksClient>> _loggerMock;
        private readonly Mock<ITokenProvider> _tokenProviderMock;

        public TaskClientShould()
        {
            _dateTimeUtilsMock = new Mock<IDatetimeUtils>(MockBehavior.Strict);
            _loggerMock = new Mock<ILogger<TasksClient>>(MockBehavior.Strict);
            _tokenProviderMock = new Mock<ITokenProvider>(MockBehavior.Strict);
        }
        
        [Fact]
        public async Task ReturnTasksWithOverdues()
        {
            _tokenProviderMock.Setup(x => x.GetToken()).Returns(Guid.NewGuid().ToString());

            DateTime dateTime = DateTime.Today;
            
            _dateTimeUtilsMock.Setup(x => x.GetEndOfDay(dateTime)).Returns(dateTime);

            _dateTimeUtilsMock.Setup(x => x.FormatLongUtc(dateTime)).Returns(It.IsAny<string>());

            var tasksClient = new TasksClient(new MockHttpClientFactory(), _loggerMock.Object, 
                _dateTimeUtilsMock.Object, _tokenProviderMock.Object);

            ResponseModel<TaskModel> tasks = await tasksClient.GetTasks(dueDatetime: dateTime,
                includeOverdues: true,
                fields: Constants.SelectedTaskFields);

            var taskModel = tasks.Value.First();

            Assert.Equal(TestDataHelper.Tasks.Value.First().Subject, taskModel.Subject);
        }
        
        [Fact]
        public async Task ReturnTasksWithoutOverdues()
        {
           
            _tokenProviderMock.Setup(x => x.GetToken()).Returns(Guid.NewGuid().ToString());

            DateTime dateTime = DateTime.Today;
            
            _dateTimeUtilsMock.Setup(x => x.GetStartOfDay(dateTime)).Returns(dateTime);
            _dateTimeUtilsMock.Setup(x => x.GetEndOfDay(dateTime)).Returns(dateTime);

            _dateTimeUtilsMock.Setup(x => x.FormatLongUtc(dateTime)).Returns(It.IsAny<string>());

            var tasksClient = new TasksClient(new MockHttpClientFactory(), _loggerMock.Object, 
                _dateTimeUtilsMock.Object, _tokenProviderMock.Object);

            ResponseModel<TaskModel> tasks = await tasksClient.GetTasks(dueDatetime: dateTime,
                includeOverdues: false,
                fields: Constants.SelectedTaskFields);

            var taskModel = tasks.Value.First();

            Assert.Equal(TestDataHelper.Tasks.Value.First().Subject, taskModel.Subject);
        }

        public void Dispose()
        {
            _loggerMock.VerifyAll();
            _dateTimeUtilsMock.VerifyAll();
            _tokenProviderMock.VerifyAll();
        }
    }
}
