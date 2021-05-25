using gRPC.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gRPC.WpfClient.Services
{
    public interface IUserService
    {
        Task<Result> AddUser(UserModel user);
        Task<Result> DeleteUser(int Id);
        Task<Result<IEnumerable<UserModel>>> GetUsers();
        Task<Result> SaveEditedUser(UserModel user);
    }
}