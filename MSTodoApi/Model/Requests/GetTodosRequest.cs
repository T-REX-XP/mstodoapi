using System;

namespace MSTodoApi.Model.Requests
{
    public class GetTodosRequest
    {
        public GetTodosRequest()
        {
            IncludeOverdueTasks = false;
            IncludeCancelledEvents = false;
        }
        public DateTime DueDateTime { get; set; }
        public bool IncludeOverdueTasks { get; set; }
        public bool IncludeCancelledEvents { get; set; }
        public string TaskFields { get; set; }
        public string EventFields { get; set; }
    }
}