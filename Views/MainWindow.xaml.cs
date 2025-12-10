using System.Windows;
using CourseSelectionSystem.Data;
using CourseSelectionSystem.Models;
using CourseSelectionSystem.Services;
using CourseSelectionSystem.ViewModels;
using CourseSelectionSystem.Views.Admin;
using CourseSelectionSystem.Views.Student;

namespace CourseSelectionSystem.Views
{
    public partial class MainWindow : Window
    {
        private readonly UserService _userService; // 存储服务实例
        private readonly CourseService _courseService;
        private readonly OfferingService _offeringService;
        private readonly EnrollmentService _enrollmentService;
        private readonly User _currentUser;
        // 构造函数：必须接收登录成功的 User 对象
        public MainWindow(User user)
        {
            InitializeComponent();
            _currentUser = user; // 🔴 保存用户对象
            var context = new AppDbContext();
            _userService = new UserService(context);
            _courseService = new CourseService(context);
            _offeringService = new OfferingService(context);
            _enrollmentService = new EnrollmentService(context);

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

        public AdminOfferingView GetAdminOfferingView()
        {
            return new Views.Admin.AdminOfferingView(_offeringService);
        }

        public Views.ProfileView GetProfileView()
        {
            // 将当前用户和 Service 传递给 ProfileView
            return new Views.ProfileView(_currentUser, _userService);
        }

        public StudentSelectCourseView GetStudentSelectCourseView()
        {
            // 将 Service 和当前用户传递给 View
            return new StudentSelectCourseView(_enrollmentService, _currentUser);
        }

        // TODO: 未来添加其他视图工厂方法，如 GetAdminCourseView()
    }
}