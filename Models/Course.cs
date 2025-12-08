using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseSelectionSystem.Models
{
    public class Course
    {
        [Key]
        [Column(TypeName = "varchar(10)")]
        public string CourseID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string CourseName { get; set; }

        [Required]
        // 注意：数据库是 DECIMAL(3,1)，C# 中用 decimal
        public decimal Credit { get; set; }

        // 导航属性：一个课程可以有多个开课计划
        public ICollection<Offering> Offerings { get; set; }
    }
}
