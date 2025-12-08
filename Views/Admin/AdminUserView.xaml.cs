using System.Windows.Controls;
using CourseSelectionSystem.Services;
using CourseSelectionSystem.ViewModels.Admin;

namespace CourseSelectionSystem.Views.Admin
{
    public partial class AdminUserView : UserControl
    {
        public AdminUserView(UserService userService)
        {
            InitializeComponent();
            // 确保将 Service 注入到 ViewModel
            this.DataContext = new AdminUserViewModel(userService);
        }
    }
}