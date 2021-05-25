using gRPC.Models;
using System.Threading.Tasks;

namespace gRPC.WpfClient.Services
{
    public interface ILoginService
    {
        Task<Result> Login(string username, string password);
        void Logout();
    }
}