using System;
using System.Threading.Tasks;
using MSTodoApi.Model;

namespace MSTodoApi.Infrastructure.Http
{
    public interface IEventsClient
    {
        Task<ResponseModel<EventModel>> GetEvents( DateTime dueDateTime,
            string fields="", bool includeCancelledEvents=false);
    }
}