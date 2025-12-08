using System.Windows;
using CourseSelectionSystem.Data;
using CourseSelectionSystem.Services;
using CourseSelectionSystem.ViewModels;

namespace CourseSelectionSystem.Views
{
    public partial class RegisterView : Window
    {
        // 默认构造函数（用于 App.xaml 启动或设计器）
        public RegisterView()
        {
            InitializeComponent();
            // 注意：如果您直接通过 new RegisterView() 启动，此处的 UserService 会是新的实例，
            // 依赖注入容器解决此问题，但目前我们采用构造函数注入。
            // 建议您只使用带参数的构造函数。
        }

        // 关键构造函数：接受 UserService 实例
        public RegisterView(UserService userService)
        {
            InitializeComponent();

            // 设置 DataContext，使用传入的 UserService 实例
            this.DataContext = new RegisterViewModel(userService);
        }
    }
}