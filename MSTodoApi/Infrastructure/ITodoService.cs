using System;
using System.Threading.Tasks;
using MSTodoApi.ViewModel;

namespace MSTodoApi.Infrastructure
{
    public interface ITodoService
    {
        Task<TodosViewModel> GetTodos(DateTime dueDateTime, 
            bool includeOverdueTasks=false, 
            string taskFields = "", 
            string eventFields = "",
            bool includeCancelledEvents=false);
    }
}