using System.Windows;
using CourseSelectionSystem.Models;
using CourseSelectionSystem.ViewModels;

namespace CourseSelectionSystem.Views
{
    public partial class MainWindow : Window
    {
        // 构造函数：必须接收登录成功的 User 对象
        public MainWindow(User user)
        {
            InitializeComponent();

            // !!! 关键更新：实例化 MainViewModel 时，传递 'this' (MainWindow 实例) !!!
            this.DataContext = new MainViewModel(user, this);
        }
    }
}