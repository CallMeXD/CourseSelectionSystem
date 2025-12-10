using System.Windows.Controls;
using CourseSelectionSystem.Models;
using CourseSelectionSystem.Services;
using CourseSelectionSystem.ViewModels.Teacher;

namespace CourseSelectionSystem.Views.Teacher
{
    public partial class TeacherGradeInputView : UserControl
    {
        public TeacherGradeInputView(TeacherService teacherService, User currentUser)
        {
            InitializeComponent();
            this.DataContext = new TeacherGradeInputViewModel(teacherService, currentUser);
        }
    }
}