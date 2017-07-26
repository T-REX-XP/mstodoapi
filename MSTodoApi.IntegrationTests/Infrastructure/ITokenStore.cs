namespace MSTodoApi.IntegrationTests.Infrastructure
{
    public interface ITokenStore
    {
        string AccessToken { get; set; }
        string RefreshToken { get; set; }
        void UpdateTokens(string json);
    }
}