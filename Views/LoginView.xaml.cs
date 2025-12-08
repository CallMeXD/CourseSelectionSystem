using System.Windows;
using CourseSelectionSystem.Data;
using CourseSelectionSystem.Services;
using CourseSelectionSystem.ViewModels;

namespace CourseSelectionSystem.Views
{
    public partial class LoginView : Window
    {
        // 声明服务和上下文为私有字段
        private readonly UserService _userService;
        private readonly AppDbContext _context;

        public LoginView()
        {
            InitializeComponent();

            // 依赖项实例化 (这是必要的，因为 LoginView 是应用的第一个窗口)
            _context = new AppDbContext();
            _userService = new UserService(_context);

            // 设置 DataContext，传递 UserService 和 View 自身引用
            this.DataContext = new LoginViewModel(_userService, this);
        }

        // **!!! 关键新增方法 !!!** // 供 LoginViewModel 调用，用于安全地创建 RegisterView 实例
        public UserService GetUserService() => _userService;
    }
}