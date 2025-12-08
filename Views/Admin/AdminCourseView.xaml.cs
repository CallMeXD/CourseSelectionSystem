using System.Windows.Controls;
using CourseSelectionSystem.Services;
using CourseSelectionSystem.ViewModels.Admin;

namespace CourseSelectionSystem.Views.Admin
{
    public partial class AdminCourseView : UserControl
    {
        // 构造函数接收 CourseService
        public AdminCourseView(CourseService courseService)
        {
            InitializeComponent();
            this.DataContext = new AdminCourseViewModel(courseService);
        }
    }
}
