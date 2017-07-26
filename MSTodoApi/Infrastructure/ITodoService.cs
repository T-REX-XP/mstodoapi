using System.Threading.Tasks;
using MSTodoApi.Model.Requests;
using MSTodoApi.ViewModel;

namespace MSTodoApi.Infrastructure
{
    public interface ITodoService
    {
        Task<OperationResult<TodosViewModel>> GetTodos(GetTodosRequest request);
    }
}
