using gRPC.Api.Protos;
using gRPC.Models;
using gRPC.WpfClient.Startup;
using Grpc.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gRPC.WpfClient.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenProvider _tokenProvider;

        public UserService(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public async Task<Result<IEnumerable<UserModel>>> GetUsers()
        {
            var _grpcClient = new Users.UsersClient(CreateAuthenticatedGrpcChannel.CreateGrpcChannel(_tokenProvider));
            UsersReplay retVal;
            try
            {
                retVal = await _grpcClient.GetUsersAsync(new Empty());
            }
            catch (RpcException ex)
            {
                return Result<IEnumerable<UserModel>>.Failed(ex.Message);
            }

            var mappedUsers = retVal.Users.Select(v => new UserModel { Id = v.Id, Username = v.Username, Password = v.Password });

            return Result<IEnumerable<UserModel>>.Success(mappedUsers);
        }

        public async Task<Result> SaveEditedUser(UserModel user)
        {
            var _grpcClient = new Users.UsersClient(CreateAuthenticatedGrpcChannel.CreateGrpcChannel(_tokenProvider));
            Replay retVal;
            try
            {
                retVal = await _grpcClient.EditUserAsync(new EditRequest { Id = user.Id, Username = user.Username, Password = user.Password });
            }
            catch (RpcException ex)
            {
                return Result.Failed(ex.Message);
            }

            return (retVal.Success ? Result.Success : Result.Failed(retVal.Message));
        }

        public async Task<Result> DeleteUser(int Id)
        {
            var _grpcClient = new Users.UsersClient(CreateAuthenticatedGrpcChannel.CreateGrpcChannel(_tokenProvider));
            Replay retVal;
            try
            {
                retVal = await _grpcClient.DeleteUserAsync(new RemoveRequest { Id = Id });
            }
            catch (RpcException ex)
            {
                return Result.Failed(ex.Message);
            }

            return (retVal.Success ? Result.Success : Result.Failed(retVal.Message));
        }

        public async Task<Result> AddUser(UserModel user)
        {
            var _grpcClient = new Users.UsersClient(CreateAuthenticatedGrpcChannel.CreateGrpcChannel(_tokenProvider));
            Replay retVal;
            try
            {
                retVal = await _grpcClient.AddUserAsync(new AddRequest { Username = user.Username, Password = user.Password });
            }
            catch (RpcException ex)
            {
                return Result.Failed(ex.Message);
            }

            return (retVal.Success ? Result.Success : Result.Failed(retVal.Message));
        }
    }
}
