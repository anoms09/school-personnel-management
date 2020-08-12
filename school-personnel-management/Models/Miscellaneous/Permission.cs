using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace School.Personnel.Management.Models.Miscellaneous
{
    public class Permission
    {
        public long Id { get; set; }
        [Required] public string PermissionName { get; set; }
        [Required] public string PermissionDescription{ get; set; }
        public bool IsAppAdminRole { get; set; }
    }

    public class PermissionDto : Permission
    {
        public long TotalCount { get; set; }
    }
}
