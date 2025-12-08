using CourseSelectionSystem.Data;
using CourseSelectionSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseSelectionSystem.Services
{
    public class CourseService
    {
        private readonly AppDbContext _context;

        public CourseService(AppDbContext context)
        {
            _context = context;
        }

        // --- 1. 获取所有课程 ---
        public List<Course> GetAllCourses()
        {
            return _context.Courses.ToList();
        }

        // --- 2. 添加课程 ---
        public bool AddCourse(string courseId, string courseName, decimal credit)
        {
            try
            {
                if (_context.Courses.Any(c => c.CourseID == courseId))
                {
                    return false; // ID 已存在
                }

                var newCourse = new Course
                {
                    CourseID = courseId,
                    CourseName = courseName,
                    Credit = credit
                };

                _context.Courses.Add(newCourse);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // --- 3. 删除课程 ---
        public bool DeleteCourse(string courseId)
        {
            try
            {
                var course = _context.Courses.FirstOrDefault(c => c.CourseID == courseId);
                if (course != null)
                {
                    _context.Courses.Remove(course);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                // 如果课程已经被引用（有开课计划），删除会失败
                return false;
            }
        }
    }
}