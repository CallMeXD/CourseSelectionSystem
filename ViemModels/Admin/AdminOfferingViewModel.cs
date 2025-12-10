using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CourseSelectionSystem.Models;
using CourseSelectionSystem.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;

namespace CourseSelectionSystem.ViewModels.Admin
{
    public partial class AdminOfferingViewModel : ObservableObject
    {
        private readonly OfferingService _offeringService;

        // --- 数据列表 ---
        [ObservableProperty]
        private ObservableCollection<Offering> _offerings = new ObservableCollection<Offering>();

        // 下拉框数据源
        [ObservableProperty]
        private ObservableCollection<Course> _availableCourses = new ObservableCollection<Course>();

        [ObservableProperty]
        private ObservableCollection<User> _availableTeachers = new ObservableCollection<User>();

        // --- 选中项 ---
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteOfferingCommand))]
        private Offering _selectedOffering;

        // --- 新增输入项 ---
        [ObservableProperty]
        private Course _selectedNewCourse; // 对应 ComboBox 选中的课程

        [ObservableProperty]
        private User _selectedNewTeacher; // 对应 ComboBox 选中的教师

        [ObservableProperty]
        private string _newSemester;

        [ObservableProperty]
        private string _newLocation;

        [ObservableProperty]
        private string _newCapacityText;

        public AdminOfferingViewModel(OfferingService offeringService)
        {
            _offeringService = offeringService;
            LoadDataCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadData()
        {
            // 1. 加载开课列表
            var offeringsList = await Task.Run(() => _offeringService.GetAllOfferings());
            Offerings = new ObservableCollection<Offering>(offeringsList);

            // 2. 加载下拉框数据 (课程和教师)
            var coursesList = await Task.Run(() => _offeringService.GetAllCourses());
            AvailableCourses = new ObservableCollection<Course>(coursesList);

            var teachersList = await Task.Run(() => _offeringService.GetAllTeachers());
            AvailableTeachers = new ObservableCollection<User>(teachersList);
        }

        [RelayCommand]
        private void AddOffering()
        {
            // 校验
            if (SelectedNewCourse == null || SelectedNewTeacher == null ||
                string.IsNullOrEmpty(NewSemester) || string.IsNullOrEmpty(NewLocation))
            {
                MessageBox.Show("请填写完整信息 (课程、教师、学期、地点、容量)。", "提示");
                return;
            }

            if (!int.TryParse(NewCapacityText, out int capacity) || capacity <= 0)
            {
                MessageBox.Show("容量必须是正整数。", "错误");
                return;
            }

            // 调用服务
            bool success = _offeringService.AddOffering(
                SelectedNewCourse.CourseID,
                SelectedNewTeacher.UserID,
                NewSemester,
                NewLocation,
                capacity
            );

            if (success)
            {
                MessageBox.Show("开课计划添加成功！", "成功");
                LoadDataCommand.Execute(null); // 刷新

                // 重置部分输入 (学期通常不变，可以不重置)
                NewLocation = string.Empty;
                NewCapacityText = string.Empty;
            }
            else
            {
                MessageBox.Show("添加失败。", "错误");
            }
        }

        [RelayCommand(CanExecute = nameof(CanDeleteOffering))]
        private void DeleteOffering()
        {
            if (MessageBox.Show($"确定删除该开课计划吗？\n({SelectedOffering.Semester} - {SelectedOffering.Course.CourseName})",
                                "确认", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                bool success = _offeringService.DeleteOffering(SelectedOffering.OfferingID);
                if (success)
                {
                    MessageBox.Show("删除成功。", "提示");
                    LoadDataCommand.Execute(null);
                }
                else
                {
                    MessageBox.Show("删除失败：可能已有学生选课。", "错误");
                }
            }
        }

        private bool CanDeleteOffering() => SelectedOffering != null;
    }
}