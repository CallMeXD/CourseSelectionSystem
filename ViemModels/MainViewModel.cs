using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CourseSelectionSystem.Views;
using CourseSelectionSystem.Models;
using System.Windows; // 引入 Window
using System.Linq;
using System; // 引入 System，尽管可能不是必需，但为了安全保留

namespace CourseSelectionSystem.ViewModels
{
    // 必须是 partial
    public partial class MainViewModel : ViewModelBase
    {
        private readonly User _currentUser; // 存储当前登录的用户信息

        // !!! 关键修正 1：声明私有字段来存储窗口实例 !!!
        private readonly Window _currentWindow;

        [ObservableProperty]
        private string _welcomeMessage;

        // 侧边栏菜单项列表
        [ObservableProperty]
        private ObservableCollection<MenuItem> _menuItems;

        // 当前选中的菜单项，用于触发视图切换
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CurrentViewModel))]
        private MenuItem _selectedMenuItem;

        // 当前显示在 ContentControl 中的 ViewModel/UserControl
        public ObservableObject CurrentViewModel { get; set; }

        // !!! 关键修正 2：修改构造函数，接收 Window 实例 !!!
        public MainViewModel(User user, Window currentWindow)
        {
            _currentUser = user;
            _currentWindow = currentWindow; // 存储窗口实例
            WelcomeMessage = $"欢迎您，{user.UserName} ({user.Role})";
            LoadMenuItems();
        }

        // --- 逻辑方法 ---

        partial void OnSelectedMenuItemChanged(MenuItem value)
        {
            if (value != null)
            {
                SwitchView(value);
            }
        }

        private void LoadMenuItems()
        {
            var allMenuItems = new ObservableCollection<MenuItem>
            {
                // 管理员菜单
                new MenuItem { Name = "用户管理", Role = "Admin", ViewModelType = "AdminUserViewModel" },
                new MenuItem { Name = "课程管理", Role = "Admin", ViewModelType = "AdminCourseViewModel" },
                new MenuItem { Name = "开课计划", Role = "Admin", ViewModelType = "AdminOfferingViewModel" },
                
                // 教师菜单
                new MenuItem { Name = "任课查询", Role = "Teacher", ViewModelType = "TeacherTeachingViewModel" },
                new MenuItem { Name = "成绩录入", Role = "Teacher", ViewModelType = "TeacherGradeViewModel" },
                
                // 学生菜单
                new MenuItem { Name = "课程查询/选课", Role = "Student", ViewModelType = "StudentCourseViewModel" },
                new MenuItem { Name = "我的课程", Role = "Student", ViewModelType = "StudentEnrolledViewModel" },
                new MenuItem { Name = "成绩查询", Role = "Student", ViewModelType = "StudentGradeViewModel" },
                
                // 所有角色
                new MenuItem { Name = "个人信息", Role = "All", ViewModelType = "ProfileViewModel" }
            };

            // 过滤菜单项
            MenuItems = new ObservableCollection<MenuItem>(
                allMenuItems.Where(item => item.Role == _currentUser.Role || item.Role == "All")
            );

            // 默认选中第一个菜单项
            SelectedMenuItem = MenuItems.FirstOrDefault();
        }

        private void SwitchView(MenuItem menuItem)
        {
            // 临时演示：
            CurrentViewModel = new TemporaryViewModel(menuItem.Name);
        }

        // --- 命令实现 ---

        [RelayCommand]
        private void Logout()
        {
            // 1. 关闭主窗口
            _currentWindow.Close();

            // 2. 打开新的登录窗口。注意：注册视图的实例化应该在 LoginView.xaml.cs 中处理依赖项
            // 为了快速实现，这里直接实例化 LoginView
            var loginView = new LoginView();
            loginView.Show();
        }
    }

    // --- 临时占位符保持不变 ---
    public partial class TemporaryViewModel : ObservableObject
    {
        public string Message { get; set; }
        public TemporaryViewModel(string viewName)
        {
            Message = $"欢迎使用 {viewName} 模块！";
        }
    }
}