using CourseSelectionSystem.Data;
using CourseSelectionSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CourseSelectionSystem.Services
{
    public class EnrollmentService
    {
        private readonly AppDbContext _context;

        public EnrollmentService(AppDbContext context)
        {
            _context = context;
        }

        // --- 1. 获取所有当前学期的开课列表 ---
        public List<Offering> GetAvailableOfferings()
        {
            // 默认获取所有开课，但可以在这里添加学期过滤逻辑
            return _context.Offerings
                .Include(o => o.Course)
                .Include(o => o.Teacher)
                .ToList();
        }

        // --- 2. 获取学生的已选课程列表 ---
        public List<Enrollment> GetStudentEnrollments(string studentId)
        {
            return _context.Enrollments
                .Where(e => e.StudentID == studentId)
                .Include(e => e.Offering)
                    .ThenInclude(o => o.Course) // 包含课程信息
                .Include(e => e.Offering)
                    .ThenInclude(o => o.Teacher) // 包含教师信息
                .ToList();
        }

        // --- 3. 选课核心业务逻辑 ---
        public string EnrollCourse(string studentId, int offeringId)
        {
            var offering = _context.Offerings.FirstOrDefault(o => o.OfferingID == offeringId);
            if (offering == null)
            {
                return "开课计划不存在。";
            }

            // A. 容量校验
            var currentEnrollmentCount = _context.Enrollments.Count(e => e.OfferingID == offeringId);
            if (currentEnrollmentCount >= offering.Capacity)
            {
                return "选课失败：课程容量已满。";
            }

            // B. 重复选课校验
            if (_context.Enrollments.Any(e => e.StudentID == studentId && e.OfferingID == offeringId))
            {
                return "选课失败：您已选择该课程。";
            }

            // C. 课程重复性校验 (避免学生选择同一门基础课程的不同班级)
            var courseId = offering.CourseID;
            var existingEnrollments = _context.Enrollments
                                        .Include(e => e.Offering)
                                        .Where(e => e.StudentID == studentId)
                                        .Select(e => e.Offering.CourseID);

            if (existingEnrollments.Contains(courseId))
            {
                return "选课失败：您已选择同名课程的其他班级。";
            }

            // TODO: D. 时间冲突校验 (如果您的 Offering 模型包含时间字段，需要在这里实现)

            // E. 选课成功
            var newEnrollment = new Enrollment
            {
                StudentID = studentId,
                OfferingID = offeringId,
                EnrollmentDate = System.DateTime.Now
            };

            _context.Enrollments.Add(newEnrollment);
            _context.SaveChanges();
            return "选课成功！";
        }

        // --- 4. 退课逻辑 ---
        public string DropCourse(string studentId, int offeringId)
        {
            var enrollment = _context.Enrollments.FirstOrDefault(e => e.StudentID == studentId && e.OfferingID == offeringId);

            if (enrollment == null)
            {
                return "退课失败：您未选择该课程。";
            }

            _context.Enrollments.Remove(enrollment);
            _context.SaveChanges();
            return "退课成功！";
        }
    }
}
