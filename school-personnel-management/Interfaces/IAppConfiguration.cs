using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Personnel.Management.Interfaces
{
    public interface IAppConfiguration
    {
        string PersonnelDbConnectionString { get; }
       
    }
}
