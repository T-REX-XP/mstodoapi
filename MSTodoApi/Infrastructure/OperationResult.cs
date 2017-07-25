namespace MSTodoApi.Infrastructure
{
    public class OperationResult<T>
    {
        public T Value { get; set; }
        public bool Success => Value != null;
    }
}