using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CourseSelectionSystem.Views;
using CourseSelectionSystem.Models;
using System.Windows;
using System.Linq;
using System;
// 🟢 修正1：必须引用 Services 命名空间
using CourseSelectionSystem.Services;

namespace CourseSelectionSystem.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly User _currentUser;
        private readonly UserService _userService;
        private readonly Window _currentWindow;

        [ObservableProperty]
        private string _welcomeMessage;

        [ObservableProperty]
        private ObservableCollection<MenuItem> _menuItems;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CurrentViewModel))]
        private MenuItem _selectedMenuItem;

        // 🟢 修正2：将类型改为 object
        // 只有 object 才能同时容纳 ViewModel 和 UserControl (View)
        // 使用 [ObservableProperty] 让 MVVM 工具包自动生成 public object CurrentViewModel 属性
        [ObservableProperty]
        private object _currentViewModel;

        public MainViewModel(User user, Window currentWindow, UserService userService)
        {
            _currentUser = user;
            _currentWindow = currentWindow;
            _userService = userService;
            WelcomeMessage = $"欢迎您，{user.UserName} ({user.Role})";
            LoadMenuItems();
        }

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
                new MenuItem { Name = "用户管理", Role = "Admin", ViewModelType = "AdminUserViewModel" },
                new MenuItem { Name = "课程管理", Role = "Admin", ViewModelType = "AdminCourseViewModel" },
                new MenuItem { Name = "开课计划", Role = "Admin", ViewModelType = "AdminOfferingViewModel" },
                new MenuItem { Name = "任课查询", Role = "Teacher", ViewModelType = "TeacherTeachingViewModel" },
                new MenuItem { Name = "成绩录入", Role = "Teacher", ViewModelType = "TeacherGradeViewModel" },
                new MenuItem { Name = "课程查询/选课", Role = "Student", ViewModelType = "StudentCourseViewModel" },
                new MenuItem { Name = "我的课程", Role = "Student", ViewModelType = "StudentEnrolledViewModel" },
                new MenuItem { Name = "成绩查询", Role = "Student", ViewModelType = "StudentGradeViewModel" },
                new MenuItem { Name = "个人信息", Role = "All", ViewModelType = "ProfileViewModel" }
            };

            string userRole = _currentUser?.Role ?? "Guest";

            MenuItems = new ObservableCollection<MenuItem>(
                allMenuItems.Where(item => item.Role == userRole || item.Role == "All")
            );

            SelectedMenuItem = MenuItems.FirstOrDefault();
        }

        private void SwitchView(MenuItem menuItem)
        {
            var mainWindow = _currentWindow as MainWindow;

            if (mainWindow == null)
            {
                CurrentViewModel = new TemporaryViewModel("错误：找不到主窗口实例。");
                return;
            }

            switch (menuItem.ViewModelType)
            {
                case "AdminUserViewModel":
                    // 🟢 修正3：因为 _currentViewModel 是 object 类型，这里可以接收 UserControl 了
                    CurrentViewModel = mainWindow.GetAdminUserView();
                    break;

                case "AdminCourseViewModel":
                    CurrentViewModel = mainWindow.GetAdminCourseView();
                    break;


                default:
                    CurrentViewModel = new TemporaryViewModel(menuItem.Name);
                    break;
            }
        }

        [RelayCommand]
        private void Logout()
        {
            _currentWindow.Close();
            var loginView = new LoginView();
            loginView.Show();
        }
    }

    public partial class TemporaryViewModel : ObservableObject
    {
        public string Message { get; set; }
        public TemporaryViewModel(string viewName)
        {
            Message = $"欢迎使用 {viewName} 模块！";
        }
    }
}