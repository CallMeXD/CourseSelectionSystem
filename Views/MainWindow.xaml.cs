using System.Windows;
using CourseSelectionSystem.Data;
using CourseSelectionSystem.Models;
using CourseSelectionSystem.Services;
using CourseSelectionSystem.ViewModels;
using CourseSelectionSystem.Views.Admin;

namespace CourseSelectionSystem.Views
{
    public partial class MainWindow : Window
    {
        private readonly UserService _userService; // 存储服务实例
        private readonly CourseService _courseService;
        // 构造函数：必须接收登录成功的 User 对象
        public MainWindow(User user)
        {
            InitializeComponent();
            var context = new AppDbContext();
            _userService = new UserService(context);
            _courseService = new CourseService(context);

            // 2. 实例化 ViewModel，传递用户信息、窗口引用和 Service 实例
            // 注意：MainViewModel 构造函数需要三个参数
            this.DataContext = new MainViewModel(user, this, _userService);

        }
        public AdminUserView GetAdminUserView()
        {
            // 实例化 AdminUserView，并将 Service 注入给它的构造函数
            return new Views.Admin.AdminUserView(_userService);
        }

        public AdminCourseView GetAdminCourseView()
        {
            // 将 CourseService 注入到 View 中
            return new Views.Admin.AdminCourseView(_courseService);
        }

        // TODO: 未来添加其他视图工厂方法，如 GetAdminCourseView()
    }
}