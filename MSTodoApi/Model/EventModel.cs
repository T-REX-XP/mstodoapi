namespace MSTodoApi.Model
{
    public class TaskStart
    {
        public string DateTime { get; set; }
        public string TimeZone { get; set; }
    }

    public class TaskEnd
    {
        public string DateTime { get; set; }
        public string TimeZone { get; set; }
    }

    public class EventModel
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public TaskStart Start { get; set; }
        public TaskEnd End { get; set; }
    }
}