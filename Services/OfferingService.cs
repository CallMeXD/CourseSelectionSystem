using CourseSelectionSystem.Data;
using CourseSelectionSystem.Models;
using Microsoft.EntityFrameworkCore; // 必须引用，用于 .Include
using System.Collections.Generic;
using System.Linq;

namespace CourseSelectionSystem.Services
{
    public class OfferingService
    {
        private readonly AppDbContext _context;

        public OfferingService(AppDbContext context)
        {
            _context = context;
        }

        // --- 1. 获取所有开课信息 (包含关联的课程名和教师名) ---
        public List<Offering> GetAllOfferings()
        {
            // 使用 Include 预加载关联数据，否则 Course 和 Teacher 属性为 null
            return _context.Offerings
                .Include(o => o.Course)
                .Include(o => o.Teacher)
                .ToList();
        }

        // --- 2. 获取所有课程 (用于下拉框) ---
        public List<Course> GetAllCourses()
        {
            return _context.Courses.ToList();
        }

        // --- 3. 获取所有教师 (用于下拉框) ---
        public List<User> GetAllTeachers()
        {
            return _context.Users.Where(u => u.Role == "Teacher").ToList();
        }

        // --- 4. 添加开课计划 ---
        public bool AddOffering(string courseId, string teacherId, string semester, string location, int capacity)
        {
            try
            {
                var newOffering = new Offering
                {
                    CourseID = courseId,
                    TeacherID = teacherId,
                    Semester = semester,
                    Location = location,
                    Capacity = capacity
                };

                _context.Offerings.Add(newOffering);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // --- 5. 删除开课计划 ---
        public bool DeleteOffering(int offeringId)
        {
            try
            {
                var offering = _context.Offerings.FirstOrDefault(o => o.OfferingID == offeringId);
                if (offering != null)
                {
                    _context.Offerings.Remove(offering);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                // 如果已有学生选课，可能会删除失败 (取决于数据库约束)
                return false;
            }
        }
    }
}
