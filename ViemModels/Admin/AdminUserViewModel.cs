using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using CourseSelectionSystem.Models;
using CourseSelectionSystem.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CourseSelectionSystem.ViewModels.Admin
{
    public partial class AdminUserViewModel : ObservableObject
    {
        private readonly UserService _userService;

        // --- 绑定属性 ---
        [ObservableProperty]
        private ObservableCollection<User> _users = new ObservableCollection<User>();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteUserCommand))]
        private User _selectedUser;

        // 用于新增用户输入
        [ObservableProperty]
        private string _newUserId;
        [ObservableProperty]
        private string _newUserName;
        [ObservableProperty]
        private string _newUserRole;

        // 角色选项
        public ObservableCollection<string> Roles { get; set; } = new ObservableCollection<string> { "Student", "Teacher", "Admin" };

        public AdminUserViewModel(UserService userService)
        {
            _userService = userService;
            LoadUsersCommand.Execute(null);
        }

        // --- 命令实现 ---

        [RelayCommand]
        private async Task LoadUsers()
        {
            List<User> usersList = await Task.Run(() => _userService.GetAllUsers());
            Users = new ObservableCollection<User>(usersList);
        }

        [RelayCommand]
        private void AddUser()
        {
            if (string.IsNullOrEmpty(NewUserId) || string.IsNullOrEmpty(NewUserName) || string.IsNullOrEmpty(NewUserRole))
            {
                MessageBox.Show("ID、姓名和角色都必须填写！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 约定：管理员新增用户的初始密码统一设置为 "123456"
            bool success = _userService.Register(NewUserId, NewUserName, "123456", NewUserRole);

            if (success)
            {
                MessageBox.Show("用户添加成功，初始密码为：123456", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadUsersCommand.Execute(null);
                NewUserId = string.Empty;
                NewUserName = string.Empty;
                NewUserRole = null;
            }
            else
            {
                MessageBox.Show("用户添加失败，ID可能已存在或数据库错误。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand(CanExecute = nameof(CanDeleteUser))]
        private void DeleteUser()
        {
            if (MessageBox.Show($"确定删除用户 {SelectedUser.UserName} ({SelectedUser.UserID}) 吗？",
                                "确认删除", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                bool success = _userService.DeleteUser(SelectedUser.UserID);

                if (success)
                {
                    MessageBox.Show("用户删除成功。", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadUsersCommand.Execute(null);
                }
                else
                {
                    MessageBox.Show("用户删除失败。请检查该用户是否有关联的课程或选课记录。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanDeleteUser() => SelectedUser != null;
    }
}