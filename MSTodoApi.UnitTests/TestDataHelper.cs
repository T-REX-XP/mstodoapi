using System.IO;
using MSTodoApi.Model;
using Newtonsoft.Json;

namespace MSTodoApi.UnitTests
{
    public class TestDataHelper
    {
        public static string TasksJson = File.ReadAllText("tasks.json");
        public static string EventsJson = File.ReadAllText("events.json");

        public static ResponseModel<TaskModel> Tasks =
            JsonConvert.DeserializeObject<ResponseModel<TaskModel>>(TasksJson); 
        
        public static ResponseModel<EventModel> Events =
            JsonConvert.DeserializeObject<ResponseModel<EventModel>>(EventsJson);
    }
}