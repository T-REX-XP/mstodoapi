using System.Collections.Generic;
using MSTodoApi.Model;

namespace MSTodoApi.ViewModel
{
    public class TodosViewModel
    {
        public List<EventModel> Events { get; set; }
        public List<TaskModel> Tasks { get; set; }
    }
}