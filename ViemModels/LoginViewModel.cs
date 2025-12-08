using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;
using CourseSelectionSystem.Services;
using CourseSelectionSystem.Views;
using CourseSelectionSystem.Models;

namespace CourseSelectionSystem.ViewModels
{
    public partial class LoginViewModel : ViewModelBase
    {
        private readonly UserService _userService;
        private readonly Window _currentWindow;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string _userId;

        [ObservableProperty]
        private string _errorMessage;

        public LoginViewModel(UserService userService, Window currentWindow)
        {
            _userService = userService;
            _currentWindow = currentWindow;
        }

        // --- 命令实现 ---

        [RelayCommand]
        private void OpenRegister()
        {
            // 通过当前窗口引用，访问其 Code-Behind，获取 UserService 实例
            // 确保 RegisterView 构造函数能够接受 UserService
            var loginView = _currentWindow as LoginView;
            if (loginView != null)
            {
                var userServiceInstance = loginView.GetUserService();

                // 实例化 RegisterView 并传入 UserService
                var registerView = new RegisterView(userServiceInstance);
                registerView.Show();
            }
        }

        // ... (LoginCommand 和 CanLogin 保持不变)

        [RelayCommand(CanExecute = nameof(CanLogin))]
        private void Login(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            string password = passwordBox?.Password;
            if (string.IsNullOrEmpty(password))
            {
                ErrorMessage = "请输入密码。";
                return;
            }

            var user = _userService.Authenticate(UserId, password);

            if (user != null)
            {
                ErrorMessage = "登录成功！正在跳转...";

                // 实例化主窗口并跳转
                MainWindow mainWindow = new MainWindow(user);
                mainWindow.Show();
                _currentWindow.Close();
            }
            else
            {
                ErrorMessage = "账号或密码错误。";
            }
        }

        private bool CanLogin() => !string.IsNullOrEmpty(UserId);
    }
}