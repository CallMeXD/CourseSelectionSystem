using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseSelectionSystem.Models
{
    [Table("Users")] // 可选：如果类名和表名不一致，需要用这个特性指定
    public class User
    {
        [Key] // 指定 UserID 是主键
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string UserID { get; set; } // 对应 SQL 的 VARCHAR(20)

        [Required]
        [Column(TypeName = "varchar(128)")]
        public string PasswordHash { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string UserName { get; set; } // 对应 SQL 的 NVARCHAR(50)

        [Required]
        [Column(TypeName = "varchar(10)")]
        public string Role { get; set; } // 'Admin', 'Teacher', 'Student'

        // 导航属性 (Navigation Properties)：用于建立关联
        // 对于教师角色：他/她教的所有课程
        public ICollection<Offering> Teachings { get; set; }

        // 对于学生角色：他/她选择的所有课程
        public ICollection<Enrollment> Enrollments { get; set; }
    }

}
