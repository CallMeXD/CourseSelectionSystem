using System.Windows.Controls;
using CourseSelectionSystem.Models;
using CourseSelectionSystem.Services;
using CourseSelectionSystem.ViewModels.Student;

namespace CourseSelectionSystem.Views.Student
{
    public partial class StudentSelectCourseView : UserControl
    {
        // 接收 EnrollmentService 和当前用户
        public StudentSelectCourseView(EnrollmentService enrollmentService, User currentUser)
        {
            InitializeComponent();
            this.DataContext = new StudentSelectCourseViewModel(enrollmentService, currentUser);
        }
    }
}
