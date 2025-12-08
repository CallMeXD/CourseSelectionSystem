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
    public partial class AdminCourseViewModel : ObservableObject
    {
        private readonly CourseService _courseService;

        // --- 绑定属性 ---

        [ObservableProperty]
        private ObservableCollection<Course> _courses = new ObservableCollection<Course>();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteCourseCommand))]
        private Course _selectedCourse;

        // 输入字段
        [ObservableProperty]
        private string _newCourseId;

        [ObservableProperty]
        private string _newCourseName;

        [ObservableProperty]
        private string _newCreditText; // 使用字符串绑定学分，方便处理输入验证

        public AdminCourseViewModel(CourseService courseService)
        {
            _courseService = courseService;
            LoadCoursesCommand.Execute(null);
        }

        // --- 命令 ---

        [RelayCommand]
        private async Task LoadCourses()
        {
            List<Course> list = await Task.Run(() => _courseService.GetAllCourses());
            Courses = new ObservableCollection<Course>(list);
        }

        [RelayCommand]
        private void AddCourse()
        {
            // 1. 基础校验
            if (string.IsNullOrEmpty(NewCourseId) || string.IsNullOrEmpty(NewCourseName) || string.IsNullOrEmpty(NewCreditText))
            {
                MessageBox.Show("请填写所有课程信息。", "提示");
                return;
            }

            // 2. 学分格式校验
            if (!decimal.TryParse(NewCreditText, out decimal credit))
            {
                MessageBox.Show("学分必须是数字。", "错误");
                return;
            }

            // 3. 调用服务
            bool success = _courseService.AddCourse(NewCourseId, NewCourseName, credit);

            if (success)
            {
                MessageBox.Show("课程添加成功！", "成功");
                LoadCoursesCommand.Execute(null); // 刷新列表

                // 清空输入
                NewCourseId = string.Empty;
                NewCourseName = string.Empty;
                NewCreditText = string.Empty;
            }
            else
            {
                MessageBox.Show("添加失败：课程号可能已存在。", "错误");
            }
        }

        [RelayCommand(CanExecute = nameof(CanDeleteCourse))]
        private void DeleteCourse()
        {
            if (MessageBox.Show($"确定删除课程【{SelectedCourse.CourseName}】吗？", "确认", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                bool success = _courseService.DeleteCourse(SelectedCourse.CourseID);
                if (success)
                {
                    MessageBox.Show("删除成功。", "提示");
                    LoadCoursesCommand.Execute(null);
                }
                else
                {
                    MessageBox.Show("删除失败：该课程可能已有开课计划。", "错误");
                }
            }
        }

        private bool CanDeleteCourse() => SelectedCourse != null;
    }
}