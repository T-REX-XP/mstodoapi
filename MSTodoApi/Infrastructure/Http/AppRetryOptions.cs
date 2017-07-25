namespace MSTodoApi.Infrastructure.Http
{
    public class AppRetryOptions
    {
        public int HttpReqRetryCount { get; set; }
        public bool HandlerEnabled { get; set; }
    }
}