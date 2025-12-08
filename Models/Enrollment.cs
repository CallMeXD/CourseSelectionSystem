using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseSelectionSystem.Models
{
    public class Enrollment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EnrollmentID { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string StudentID { get; set; }

        public int OfferingID { get; set; }

        [Required]
        public DateTime EnrollDate { get; set; } // 注意：SQL Server 的 DATE 对应 C# 的 DateTime

        public int? Grade { get; set; } // 使用 int? (可空整数) 对应 SQL 的 NULLABLE INT

        // 导航属性 (Navigation Properties)

        // 1. 关联到 Users 表 (学生)
        [ForeignKey("StudentID")]
        public User Student { get; set; }

        // 2. 关联到 Offering 表 (开课)
        [ForeignKey("OfferingID")]
        public Offering Offering { get; set; }
    }
}
