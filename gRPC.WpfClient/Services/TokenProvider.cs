using gRPC.WpfClient.Events;
using Prism.Events;

namespace gRPC.WpfClient.Services
{
    public class TokenProvider : ITokenProvider
    {
        private protected string AccessToken { get; set; }

        public TokenProvider(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<SaveTokenEvent>().Subscribe(SaveToken);
            eventAggregator.GetEvent<RemoveTokenEvent>().Subscribe(RemoveToken);
        }

        public void SaveToken(string accesstoken) 
            => AccessToken = accesstoken;

        public void RemoveToken() 
            => AccessToken = string.Empty;

        public bool IsAccessToken() 
            => !string.IsNullOrWhiteSpace(AccessToken);

        public string GetAccessToken() 
            => AccessToken;

    }
}
