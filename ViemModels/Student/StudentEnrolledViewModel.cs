using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseSelectionSystem.Models;
using CourseSelectionSystem.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace CourseSelectionSystem.ViewModels.Student
{
    public partial class StudentEnrolledViewModel : ObservableObject
    {
        private readonly EnrollmentService _enrollmentService;
        private readonly string _studentId;

        [ObservableProperty]
        private ObservableCollection<Enrollment> _myEnrollments = new ObservableCollection<Enrollment>();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DropCourseCommand))]
        private Enrollment _selectedEnrollment;

        public StudentEnrolledViewModel(EnrollmentService enrollmentService, User currentUser)
        {
            _enrollmentService = enrollmentService;
            _studentId = currentUser.UserID;
            LoadEnrollmentsCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadEnrollments()
        {
            // 获取当前学生的所有选课记录
            var list = await Task.Run(() => _enrollmentService.GetStudentEnrollments(_studentId));
            MyEnrollments = new ObservableCollection<Enrollment>(list);
        }

        [RelayCommand(CanExecute = nameof(CanDropCourse))]
        private void DropCourse()
        {
            if (MessageBox.Show($"确定退选课程：{SelectedEnrollment.Offering.Course.CourseName} 吗？",
                                "退课确认", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                string result = _enrollmentService.DropCourse(_studentId, SelectedEnrollment.OfferingID);
                MessageBox.Show(result, "提示");

                // 刷新列表
                LoadEnrollmentsCommand.Execute(null);
            }
        }

        private bool CanDropCourse() => SelectedEnrollment != null;
    }
}