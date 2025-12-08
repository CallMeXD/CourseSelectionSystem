using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CourseSelectionSystem.Models; // 假设您的模型放在 Models 命名空间下

namespace CourseSelectionSystem.Data // 推荐将 DbContext 放在单独的 Data 命名空间下
{
    public class AppDbContext : DbContext
    {
        // -------------------------------------------------------------------
        // 1. DbSet 属性：映射数据库中的表
        // -------------------------------------------------------------------
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Offering> Offerings { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        // -------------------------------------------------------------------
        // 2. OnConfiguring：配置数据库连接
        // -------------------------------------------------------------------
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // !!! 重要：请替换为您的实际 SQL Server 连接字符串 !!!

            // 示例 (Windows 身份验证)：
            //string connectionString = "Server=LAPTOP-463A50SH;Database=CourseSelectionDB;Trusted_Connection=True;TrustServerCertificate=True;";

            // 示例 (SQL Server 身份验证):
            string connectionString = "Server=LAPTOP-463A50SH;Database=CourseSelectionDB;User Id=sa;Password=xdrush18.;TrustServerCertificate=True;"; 

            optionsBuilder.UseSqlServer(connectionString);
        }

        // -------------------------------------------------------------------
        // 3. OnModelCreating：使用 Fluent API 配置关系和约束
        // -------------------------------------------------------------------
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // --- User 配置 ---
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserID); // 明确设置主键

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>(); // 确保 Role 字段以字符串形式存储 (Admin/Teacher/Student)

            // --- Offering 配置 (教师和课程的外键关系) ---

            // 1. Offering 与 Teacher (User) 的关系：一个教师可以教授多个开课
            modelBuilder.Entity<Offering>()
                .HasOne(o => o.Teacher) // Offering 拥有一个 Teacher
                .WithMany(u => u.Teachings) // User (Teacher) 拥有多个 Teachings 集合 (在 User.cs 中定义)
                .HasForeignKey(o => o.TeacherID) // 外键是 TeacherID
                                                 // OnDelete(DeleteBehavior.Restrict) 防止意外删除有任课的教师
                .OnDelete(DeleteBehavior.Restrict);

            // 2. Offering 与 Course 的关系：一个课程代码可以有多个开课计划
            modelBuilder.Entity<Offering>()
                .HasOne(o => o.Course)
                .WithMany(c => c.Offerings)
                .HasForeignKey(o => o.CourseID)
                .OnDelete(DeleteBehavior.Cascade); // 如果删除 Course，相关的 Offering 也应该被删除

            // --- Enrollment 配置 (选课记录) ---

            // 1. Enrollment 与 Student (User) 的关系：一个学生可以有多个选课记录
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(u => u.Enrollments) // User (Student) 拥有多个 Enrollments 集合 (在 User.cs 中定义)
                .HasForeignKey(e => e.StudentID)
                .OnDelete(DeleteBehavior.Cascade); // 如果学生账号被删除，选课记录也被删除

            // 2. Enrollment 与 Offering 的关系：一个开课计划可以有多个选课记录
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Offering)
                .WithMany(o => o.Enrollments)
                .HasForeignKey(e => e.OfferingID)
                .OnDelete(DeleteBehavior.Restrict); // 如果有选课记录，限制删除该开课计划

            // 确保基类方法被调用
            base.OnModelCreating(modelBuilder);
        }
    }
}