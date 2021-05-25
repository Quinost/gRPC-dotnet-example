using gRPC.WpfClient.Events;
using gRPC.WpfClient.Services;
using Prism.Commands;
using Prism.Events;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace gRPC.WpfClient.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly ILoginService _loginService;
        private readonly IEventAggregator _eventAggregator;
        private string username;
        private string password;

        public ICommand LoginCommand { get; }

        public string Username
        {
            get => username;
            set
            {
                username = value;
                ((DelegateCommand)LoginCommand).RaiseCanExecuteChanged();
            }
        }
        public string Password 
        { 
            get => password;
            set
            {
                password = value;
                ((DelegateCommand)LoginCommand).RaiseCanExecuteChanged();
            }
        }


        public LoginViewModel(ILoginService loginService, IEventAggregator eventAggregator)
        {
            _loginService = loginService;
            _eventAggregator = eventAggregator;
            LoginCommand = new DelegateCommand(async () => await OnLoginExecute(), CanLoginExecute);
        }

        private async Task OnLoginExecute()
        {
             var result = await _loginService.Login(Username, Password);
            if (result.Succeeded)
                _eventAggregator.GetEvent<NavigationEvent>().Publish(typeof(UserViewModel));
            else
                MessageBox.Show(result.ToString());
        }

        private bool CanLoginExecute()
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }
    }
}
