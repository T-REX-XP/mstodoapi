using System.Net.Http;

namespace MSTodoApi.Infrastructure.Http
{
    public interface IHttpClientFactory
    {
        HttpClient GetClient();
    }
}