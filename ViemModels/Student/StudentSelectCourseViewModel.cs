using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseSelectionSystem.Models;
using CourseSelectionSystem.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;

namespace CourseSelectionSystem.ViewModels.Student
{
    public partial class StudentSelectCourseViewModel : ObservableObject
    {
        private readonly EnrollmentService _enrollmentService;
        private readonly string _studentId;

        // --- 绑定列表 ---
        [ObservableProperty]
        private ObservableCollection<Offering> _availableOfferings = new ObservableCollection<Offering>();

        // --- 选中项 ---
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SelectCourseCommand))]
        private Offering _selectedOffering;

        public StudentSelectCourseViewModel(EnrollmentService enrollmentService, User currentUser)
        {
            _enrollmentService = enrollmentService;
            _studentId = currentUser.UserID;
            LoadOfferingsCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadOfferings()
        {
            var list = await Task.Run(() => _enrollmentService.GetAvailableOfferings());
            AvailableOfferings = new ObservableCollection<Offering>(list);
        }

        private bool CanSelectCourse() => SelectedOffering != null;

        [RelayCommand(CanExecute = nameof(CanSelectCourse))]
        private void SelectCourse()
        {
            if (SelectedOffering == null) return;

            if (MessageBox.Show($"确定选择课程：{SelectedOffering.Course.CourseName} ({SelectedOffering.Teacher.UserName}) 吗？",
                                "确认选课", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                // 调用服务执行选课逻辑
                string result = _enrollmentService.EnrollCourse(_studentId, SelectedOffering.OfferingID);

                MessageBox.Show(result, "选课结果");

                // 刷新列表 (容量可能会变)
                LoadOfferingsCommand.Execute(null);
            }
        }
    }
}