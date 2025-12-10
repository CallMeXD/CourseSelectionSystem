using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseSelectionSystem.Models;
using CourseSelectionSystem.Services;
using System.Data;
using System.Windows;

namespace CourseSelectionSystem.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private readonly User _currentUser;

        // --- 只读显示属性 ---
        [ObservableProperty]
        private string _userId;

        [ObservableProperty]
        private string _role;

        // --- 可编辑属性 ---
        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private string _newPassword; // 绑定到密码框 (实际需用 PasswordBox，这里简化为 TextBox 或使用参数传递)

        public ProfileViewModel(UserService userService, User currentUser)
        {
            _userService = userService;
            _currentUser = currentUser;

            // 初始化显示数据
            UserId = currentUser.UserID;
            Role = currentUser.Role;
            UserName = currentUser.UserName;
        }

        [RelayCommand]
        private void SaveChanges(object parameter)
        {
            // 处理密码输入 (如果是 PasswordBox)
            string passwordToUpdate = null;
            if (parameter is System.Windows.Controls.PasswordBox pwdBox)
            {
                passwordToUpdate = pwdBox.Password;
            }

            // 调用 Service 更新
            bool success = _userService.UpdateProfile(UserId, UserName, passwordToUpdate);

            if (success)
            {
                MessageBox.Show("个人信息更新成功！\n(部分更改可能需要重新登录生效)", "成功", MessageBoxButton.OK, MessageBoxImage.Information);

                // 同步更新内存中的 currentUser 对象
                _currentUser.UserName = UserName;

                // 清空密码框
                if (parameter is System.Windows.Controls.PasswordBox pb)
                {
                    pb.Clear();
                }
            }
            else
            {
                MessageBox.Show("更新失败。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}