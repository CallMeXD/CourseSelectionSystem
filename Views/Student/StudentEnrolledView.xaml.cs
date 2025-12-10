using System.Windows.Controls;
using CourseSelectionSystem.Models;
using CourseSelectionSystem.Services;
using CourseSelectionSystem.ViewModels.Student;

namespace CourseSelectionSystem.Views.Student
{
    public partial class StudentEnrolledView : UserControl
    {
        public StudentEnrolledView(EnrollmentService enrollmentService, User currentUser)
        {
            InitializeComponent();
            this.DataContext = new StudentEnrolledViewModel(enrollmentService, currentUser);
        }
    }
}