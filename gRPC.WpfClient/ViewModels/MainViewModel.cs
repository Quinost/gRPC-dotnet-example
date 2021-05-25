using gRPC.WpfClient.Events;
using gRPC.WpfClient.Services;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace gRPC.WpfClient.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase currentView;
        public LoginViewModel LoginViewModel { get; }
        public UserViewModel UserViewModel { get; }

        private readonly ITokenProvider _tokenProvider;

        public MainViewModel(LoginViewModel loginViewModel, UserViewModel userViewModel, IEventAggregator eventAggregator, ITokenProvider tokenProvider)
        {
            LoginViewModel = loginViewModel;
            UserViewModel = userViewModel;
            _tokenProvider = tokenProvider;
            eventAggregator.GetEvent<NavigationEvent>().Subscribe(Navigate);
        }

        private void Navigate(Type obj)
        {
            switch (obj)
            {
                case Type loginType when loginType == typeof(LoginViewModel):
                    if (_tokenProvider.IsAccessToken())
                        break;
                    CurrentView = LoginViewModel;
                    break;
                case Type userType when userType == typeof(UserViewModel):
                    CurrentView = UserViewModel;
                    break;
                default:
                    MessageBox.Show("Error while navigating");
                    break;
            }
            CurrentView.Initialize();
        }

        public ViewModelBase CurrentView
        {
            get => currentView;
            set
            {
                currentView = value;
                OnPropertyChanged();
            }
        }

        public void Load()
        {
            Navigate(typeof(LoginViewModel));
        }
    }
}
