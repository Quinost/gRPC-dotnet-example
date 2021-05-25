using gRPC.Models;
using gRPC.WpfClient.Services;
using Prism.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace gRPC.WpfClient.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private UserModel editUser;
        private List<UserModel> userList;

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public List<UserModel> UserList 
        { 
            get => userList;
            set 
            {
                userList = value;
                OnPropertyChanged();
            }
        }

        public UserModel EditUser
        {
            get => editUser;
            set
            {
                editUser = value;
                OnPropertyChanged();
                if(editUser is not null)
                {
                    Editing = true;
                    OnPropertyChanged(nameof(Editing));
                }
                else
                {
                    Editing = false;
                    OnPropertyChanged(nameof(Editing));
                }
            }
        }
        public bool Editing { get; set; }
        public UserViewModel(IUserService userService)
        {
            Editing = false;
            _userService = userService;
            EditCommand = new DelegateCommand<int?>(OnEditExecute);
            DeleteCommand = new DelegateCommand<int?>(async(obj) => await OnDeleteExecute(obj));
            AddCommand = new DelegateCommand(OnAddExecute);
            SaveCommand = new DelegateCommand(async() => await OnSaveExecute());
            CancelCommand = new DelegateCommand(() => EditUser = null);
        }

        private async Task OnSaveExecute()
        {
            Result retVal;
            if (EditUser.Id != 0)
                retVal = await _userService.SaveEditedUser(EditUser);
            else
                retVal = await _userService.AddUser(EditUser);

            if (retVal.Succeeded)
            {
                EditUser = null;
                await Initialize();
            }
            else
                MessageBox.Show(retVal.ToString());
        }

        private void OnEditExecute(int? obj)
        {
            if (obj.HasValue)
                EditUser = UserList.FirstOrDefault(v => v.Id == obj.Value);
        }

        private async Task OnDeleteExecute(int? obj)
        {
            Result retVal = new();
            if (obj.HasValue)
                retVal = await _userService.DeleteUser(obj.Value);

            if (retVal.Succeeded)
            {
                EditUser = null;
                await Initialize();
            }
            else
                MessageBox.Show(retVal.ToString());
        }

        private void OnAddExecute()
        {
            if(EditUser is null)
                EditUser = new UserModel();
        }

        public override async Task Initialize()
        {
            var users = await _userService.GetUsers();
            UserList = new List<UserModel>(users.RetVal);
        }
    }
}
