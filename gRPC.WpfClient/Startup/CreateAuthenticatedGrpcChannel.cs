using gRPC.WpfClient.Services;
using Grpc.Core;
using Grpc.Net.Client;
using System.Threading.Tasks;

namespace gRPC.WpfClient.Startup
{
    public static class CreateAuthenticatedGrpcChannel
    {
        public static GrpcChannel CreateGrpcChannel(ITokenProvider tokenProvider) => GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
        {
            Credentials = ChannelCredentials.Create(new SslCredentials(), CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (tokenProvider.IsAccessToken())
                {
                    metadata.Add("Authorization", $"Bearer {tokenProvider.GetAccessToken()}");
                }
                return Task.CompletedTask;
            }))
        });
    }
}
