using Autofac;
using gRPC.Api.Protos;
using gRPC.WpfClient.Services;
using gRPC.WpfClient.ViewModels;
using Grpc.Core;
using Grpc.Net.Client;
using Prism.Events;
using System.Threading.Tasks;

namespace gRPC.WpfClient.Startup
{
    public class Bootstraper
    {
        public Bootstraper()
        {
        }
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<TokenProvider>().As<ITokenProvider>().SingleInstance();

            builder.RegisterType<LoginService>().As<ILoginService>();
            builder.RegisterType<UserService>().As<IUserService>();

            builder.Register(x => new Login.LoginClient(GrpcChannel.ForAddress("https://localhost:5001"))).AsSelf();
            builder.Register(x => new Users.UsersClient(GrpcChannel.ForAddress("https://localhost:5001"))).AsSelf();

            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<LoginViewModel>().AsSelf();
            builder.RegisterType<UserViewModel>().AsSelf();

            builder.RegisterType<MainWindow>().AsSelf();

            return builder.Build();
        }
    }
}
