using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseSelectionSystem.Models
{
    public class Offering
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // 数据库自增
        public int OfferingID { get; set; }

        [Column(TypeName = "varchar(10)")]
        // 外键 CourseID
        public string CourseID { get; set; }

        [Column(TypeName = "varchar(20)")]
        // 外键 TeacherID
        public string TeacherID { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string Semester { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Location { get; set; }

        // 导航属性 (Navigation Properties)：用于建立关联

        // 1. 关联到 Course 表 (多对一关系)
        [ForeignKey("CourseID")]
        public Course Course { get; set; }

        // 2. 关联到 Users 表 (教师)
        [ForeignKey("TeacherID")]
        public User Teacher { get; set; }

        // 3. 关联到 Enrollment 表 (一对多关系)
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
