using CourseSelectionSystem.Data;
using CourseSelectionSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CourseSelectionSystem.Services
{
    public class TeacherService
    {
        private readonly AppDbContext _context;

        public TeacherService(AppDbContext context)
        {
            _context = context;
        }

        // --- 1. 任课查询：获取某教师的所有开课计划 ---
        public List<Offering> GetTeacherOfferings(string teacherId)
        {
            return _context.Offerings
                .Where(o => o.TeacherID == teacherId)
                .Include(o => o.Course)
                .ToList();
        }

        // --- 2. 获取某门课的学生选课记录（用于成绩录入） ---
        public List<Enrollment> GetEnrollmentsByOffering(int offeringId)
        {
            // 加载选课记录，并包含关联的学生信息
            return _context.Enrollments
                .Where(e => e.OfferingID == offeringId)
                .Include(e => e.Student) // 包含学生姓名和ID
                .ToList();
        }

        // --- 3. 批量更新学生成绩 ---
        public bool UpdateGrades(List<Enrollment> enrollmentsToUpdate)
        {
            try
            {
                // 注意：这里假设传入的 enrollmentsToUpdate 已经是被 EF 追踪的对象
                // 如果是从 DataGrid 中独立创建的对象，则需要先 Attach 或 Find 后再修改。

                foreach (var updatedEnrollment in enrollmentsToUpdate)
                {
                    // 查找数据库中的原始记录
                    var originalEnrollment = _context.Enrollments
                        .Find(updatedEnrollment.EnrollmentID);

                    if (originalEnrollment != null)
                    {
                        // 仅更新 Grade 字段
                        originalEnrollment.Grade = updatedEnrollment.Grade;

                        // 标记为已修改 (如果EF追踪不到，需要显式标记)
                        _context.Entry(originalEnrollment).State = EntityState.Modified;
                    }
                }

                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
