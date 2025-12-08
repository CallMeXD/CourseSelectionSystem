using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseSelectionSystem.Models
{
    public class MenuItem
    {
        public string Name { get; set; }        // 菜单项名称 (e.g., "课程管理")
        public string Role { get; set; }        // 菜单项所需角色 (e.g., "Admin")
        public string ViewModelType { get; set; } // 对应的 ViewModel 类型名称 (e.g., "AdminUserViewModel")
    }
}
