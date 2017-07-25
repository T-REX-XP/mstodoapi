namespace MSTodoApi.Infrastructure
{
    public class OperationResult<T>
    {
        public T Result { get; set; }
        public bool Success { get; set; }
    }
}