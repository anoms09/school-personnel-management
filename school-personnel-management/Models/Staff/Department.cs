using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace School.Personnel.Management.Models.Staff
{
    public class Department
    {
        public long Id { get; set; }
        [Required]public string DeptName { get; set; }
        public string DeptDescription { get; set; }
        [Required]public string DeptCode{ get; set; }
        [Required]public string FacultyCode{ get; set; }
        
    }

    public class DepartmentDto : Department
    {
        public long TotalCount { get; set; }
    }
}
