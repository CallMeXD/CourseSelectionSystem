using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;
using CourseSelectionSystem.Services;
using System.Collections.ObjectModel;

namespace CourseSelectionSystem.ViewModels
{
    public partial class RegisterViewModel : ViewModelBase
    {
        private readonly UserService _userService;

        [ObservableProperty]
        private string _userId;

        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private string _errorMessage;

        // 角色列表
        public ObservableCollection<string> Roles { get; set; } = new ObservableCollection<string> { "Student", "Teacher" };

        [ObservableProperty]
        private string _selectedRole = "Student"; // 默认角色

        public RegisterViewModel(UserService userService)
        {
            _userService = userService;
        }

        [RelayCommand]
        private void CloseWindow(Window window)
        {
            window?.Close();
        }

        [RelayCommand]
        private void Register(object parameter)
        {
            object[] parameters = parameter as object[];
            if (parameters == null || parameters.Length != 3) return;

            // 获取控件和窗口
            var pwdBox = parameters[0] as PasswordBox;
            var confirmPwdBox = parameters[1] as PasswordBox;
            var window = parameters[2] as Window;

            string password = pwdBox?.Password;
            string confirmPassword = confirmPwdBox?.Password;

            if (string.IsNullOrEmpty(password) || password != confirmPassword)
            {
                ErrorMessage = "两次密码不一致或密码为空！";
                return;
            }
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(UserName))
            {
                ErrorMessage = "账号和姓名不能为空！";
                return;
            }

            // 调用 Service 注册
            bool success = _userService.Register(UserId, UserName, password, SelectedRole);

            if (success)
            {
                MessageBox.Show("注册成功！请返回登录。", "成功");
                window?.Close();
            }
            else
            {
                ErrorMessage = "注册失败！账号可能已存在，或数据错误。";
            }
        }
    }
}