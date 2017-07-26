using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using MSTodoApi.Infrastructure;
using MSTodoApi.Infrastructure.Auth;
using MSTodoApi.Infrastructure.Http;
using MSTodoApi.Infrastructure.Utils;
using Xunit;

namespace MSTodoApi.UnitTests
{
    public class EventsClientShould :IDisposable
    {
        readonly Mock<IDatetimeUtils> _dateTimeUtilsMock ;
        readonly Mock<ILogger<EventsClient>> _loggerMock;
        readonly Mock<ITokenProvider> _tokenProviderMock;

        public EventsClientShould()
        {
             _dateTimeUtilsMock = new Mock<IDatetimeUtils>(MockBehavior.Strict);
            _loggerMock = new Mock<ILogger<EventsClient>>(MockBehavior.Strict);
            _tokenProviderMock = new Mock<ITokenProvider>(MockBehavior.Strict);
        }
        
        [Fact]
        public async Task ReturnEvents()
        {
            DateTime dateTime = DateTime.Today;
            
            _dateTimeUtilsMock.Setup(x => x.GetEndOfDay(dateTime)).Returns(dateTime);
            _dateTimeUtilsMock.Setup(x => x.GetStartOfDay(dateTime)).Returns(dateTime);

            _tokenProviderMock.Setup(x => x.GetToken()).Returns(Guid.NewGuid().ToString());
            
            _dateTimeUtilsMock.Setup(x => x.FormatLongUtc(dateTime)).Returns(It.IsAny<string>());

            var client = new EventsClient(new MockHttpClientFactory(), _loggerMock.Object,
                _dateTimeUtilsMock.Object,_tokenProviderMock.Object);

            var events = await client.GetEvents(dueDateTime: dateTime, 
                fields: Constants.SelectedEventFields);

            var eventModel = events.Value.First();

            Assert.Equal(TestDataHelper.Events.Value.First().Subject, eventModel.Subject);
        }

        public void Dispose()
        {
            _loggerMock.VerifyAll();
            _dateTimeUtilsMock.VerifyAll();
            _tokenProviderMock.VerifyAll();
        }
    }
}