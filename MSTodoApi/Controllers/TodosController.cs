using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MSTodoApi.Infrastructure;
using MSTodoApi.ViewModel;

namespace MSTodoApi.Controllers
{
    [Route("api/[controller]")]
    public class TodosController : Controller
    {
        private readonly ITodoService _todoService;

        public TodosController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            OperationResult<TodosViewModel> operationResult = await _todoService.GetTodos(
                dueDateTime: DateTime.Today,
                includeOverdueTasks: true,
                taskFields: Constants.SelectedTaskFields,
                eventFields: Constants.SelectedEventFields);

            if (operationResult.Success)
            {
                return Ok(operationResult.Value);
            }

            return BadRequest();
        }
    }
}
