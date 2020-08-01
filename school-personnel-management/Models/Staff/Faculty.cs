using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace School.Personnel.Management.Models.Staff
{
    public class Faculty
    {
        public long Id { get; set; }
        [Required]public string FacultyName { get; set; }
        public string FacultyDescription { get; set; }
        [Required] public string FacultyCode { get; set; }      
    }

    public class FacultyDto : Faculty
    {
        public long TotalCount { get; set; }
    }
}
