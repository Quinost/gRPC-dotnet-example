using gRPC.Api.Protos;
using gRPC.Models;
using gRPC.WpfClient.Events;
using Grpc.Core;
using Prism.Events;
using System.Threading.Tasks;

namespace gRPC.WpfClient.Services
{
    public class LoginService : ILoginService
    {
        private readonly Login.LoginClient _grpcClient;
        private readonly IEventAggregator _eventAggregator;

        public LoginService(Login.LoginClient grpcClient, IEventAggregator eventAggregator)
        {
            _grpcClient = grpcClient;
            _eventAggregator = eventAggregator;
        }
        public async Task<Result> Login(string username, string password)
        {
            LoginReplay retVal;
            try
            {
                retVal = await _grpcClient.LoginAsync(new LoginRequest { Username = username, Password = password });
            }
            catch (RpcException ex)
            {
                return Result.Failed(ex.Message);
            }

            if (string.IsNullOrWhiteSpace(retVal.AccessToken))
                return Result.Failed("Error while trying login");

            _eventAggregator.GetEvent<SaveTokenEvent>().Publish(retVal.AccessToken);

            return Result.Success;
        }

        public void Logout()
        {
            _eventAggregator.GetEvent<RemoveTokenEvent>().Publish();
        }
    }
}
