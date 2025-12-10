using System.Windows.Controls;
using CourseSelectionSystem.Models;
using CourseSelectionSystem.Services;
using CourseSelectionSystem.ViewModels;

namespace CourseSelectionSystem.Views
{
    public partial class ProfileView : UserControl
    {
        // 构造函数接收 User 和 Service
        public ProfileView(User currentUser, UserService userService)
        {
            InitializeComponent();
            this.DataContext = new ProfileViewModel(userService, currentUser);
        }
    }
}