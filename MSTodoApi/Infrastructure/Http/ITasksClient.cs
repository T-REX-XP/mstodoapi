using System;
using System.Threading.Tasks;
using MSTodoApi.Model;

namespace MSTodoApi.Infrastructure.Http
{
    public interface ITasksClient
    {
        Task<ResponseModel<TaskModel>> GetTasks(
            DateTime dueDatetime,
            bool includeOverdues = false,
            string fields = "");
    }
}