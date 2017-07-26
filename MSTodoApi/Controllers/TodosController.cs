using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MSTodoApi.Infrastructure;
using MSTodoApi.Model.Requests;

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
            GetTodosRequest request = new GetTodosRequest
            {
                DueDateTime = DateTime.Today,
                IncludeOverdueTasks = true,
                TaskFields = Constants.SelectedTaskFields,
                EventFields = Constants.SelectedEventFields
            };
            
            var operationResult = await _todoService.GetTodos(request);

            if (operationResult.Success)
            {
                return Ok(operationResult.Value);
            }

            return BadRequest();
        }
    }
}
