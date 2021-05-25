namespace gRPC.WpfClient.Services
{
    public interface ITokenProvider
    {
        bool IsAccessToken();
        void RemoveToken();
        void SaveToken(string token);
        string GetAccessToken();
    }
}