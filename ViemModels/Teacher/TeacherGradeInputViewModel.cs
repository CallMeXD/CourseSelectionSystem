using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseSelectionSystem.Models;
using CourseSelectionSystem.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;

namespace CourseSelectionSystem.ViewModels.Teacher
{
    public partial class TeacherGradeInputViewModel : ObservableObject
    {
        private readonly TeacherService _teacherService;
        private readonly string _teacherId;

        // --- 绑定列表 ---
        [ObservableProperty]
        private ObservableCollection<Offering> _teacherOfferings = new ObservableCollection<Offering>(); // 教师任课列表

        [ObservableProperty]
        private ObservableCollection<Enrollment> _studentEnrollments; // 某门课的学生列表

        // --- 选中项 ---
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoadStudentsCommand))]
        private Offering _selectedOffering;

        public TeacherGradeInputViewModel(TeacherService teacherService, User currentUser)
        {
            _teacherService = teacherService;
            _teacherId = currentUser.UserID;
            LoadOfferingsCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadOfferings()
        {
            var list = await Task.Run(() => _teacherService.GetTeacherOfferings(_teacherId));
            TeacherOfferings = new ObservableCollection<Offering>(list);
            StudentEnrollments = null; // 清空学生列表
        }

        private bool CanLoadStudents() => SelectedOffering != null;

        [RelayCommand(CanExecute = nameof(CanLoadStudents))]
        private async Task LoadStudents()
        {
            if (SelectedOffering == null) return;

            var list = await Task.Run(() => _teacherService.GetEnrollmentsByOffering(SelectedOffering.OfferingID));
            StudentEnrollments = new ObservableCollection<Enrollment>(list);
        }

        [RelayCommand]
        private void SaveGrades()
        {
            if (StudentEnrollments == null || !StudentEnrollments.Any())
            {
                MessageBox.Show("没有需要保存的成绩。", "提示");
                return;
            }

            // ⚠️ 重要：将 ObservableCollection 转换为 List 传递给服务层
            bool success = _teacherService.UpdateGrades(StudentEnrollments.ToList());

            if (success)
            {
                MessageBox.Show("成绩保存成功！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("成绩保存失败，请检查输入或数据库连接。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}