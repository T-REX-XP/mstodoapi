namespace MSTodoApi.Infrastructure.Auth
{
    public interface ITokenProvider
    {
        string GetToken();
    }
}