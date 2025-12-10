using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CourseSelectionSystem.Views;
using CourseSelectionSystem.Models;
using System.Windows;
using System.Linq;
using System;
using CourseSelectionSystem.Services; // 引用 Services

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
                // Admin
                new MenuItem { Name = "用户管理", Role = "Admin", ViewModelType = "AdminUserViewModel" },
                new MenuItem { Name = "课程管理", Role = "Admin", ViewModelType = "AdminCourseViewModel" },
                new MenuItem { Name = "开课计划", Role = "Admin", ViewModelType = "AdminOfferingViewModel" },
                
                // Teacher
                new MenuItem { Name = "任课查询", Role = "Teacher", ViewModelType = "TeacherGradeInputViewModel" },
                new MenuItem { Name = "成绩录入", Role = "Teacher", ViewModelType = "TeacherGradeInputViewModel" },
                
                // Student (注意：这里 ViewModelType 名称要对应 switch 中的 case)
                new MenuItem { Name = "课程查询/选课", Role = "Student", ViewModelType = "StudentSelectCourseViewModel" },
                new MenuItem { Name = "我的课程", Role = "Student", ViewModelType = "StudentEnrolledViewModel" },
                new MenuItem { Name = "成绩查询", Role = "Student", ViewModelType = "StudentGradeViewModel" },
                
                // Common
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
                // --- 管理员视图 ---
                case "AdminUserViewModel":
                    CurrentViewModel = mainWindow.GetAdminUserView();
                    break;

                case "AdminCourseViewModel":
                    CurrentViewModel = mainWindow.GetAdminCourseView();
                    break;

                case "AdminOfferingViewModel":
                    CurrentViewModel = mainWindow.GetAdminOfferingView();
                    break;

                // --- 学生视图 ---
                // 🔴 新增：处理学生选课
                case "StudentSelectCourseViewModel":
                    // 确保 MainWindow.xaml.cs 中已经添加了 GetStudentSelectCourseView() 方法
                    CurrentViewModel = mainWindow.GetStudentSelectCourseView();
                    break;

                // 🔴 新增：处理我的课程 (阶段 6 Part 2 内容)
                case "StudentEnrolledViewModel":
                    // 如果您还未完成 Part 2，可以先注释掉下面这行，用 TemporaryViewModel 代替
                    // CurrentViewModel = new TemporaryViewModel("我的课程 (开发中)");
                    CurrentViewModel = mainWindow.GetStudentEnrolledView();
                    break;

                // 🔴 新增：处理成绩查询
                case "StudentGradeViewModel":
                    // 暂时复用我的课程视图，或者使用 TemporaryViewModel
                    CurrentViewModel = mainWindow.GetStudentEnrolledView();
                    break;
                //--
                //---教师视图---
                case "TeacherGradeInputViewModel":
                    CurrentViewModel = mainWindow.GetTeacherGradeInputView();
                    break;


                // --- 通用视图 ---
                case "ProfileViewModel":
                    CurrentViewModel = mainWindow.GetProfileView();
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