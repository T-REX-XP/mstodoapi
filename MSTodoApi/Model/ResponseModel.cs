using System.Collections.Generic;

namespace MSTodoApi.Model
{
    public class ResponseModel<T>
    {
        public List<T> Value { get; set; }
    }
}