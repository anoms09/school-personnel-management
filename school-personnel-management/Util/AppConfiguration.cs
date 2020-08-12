using Microsoft.Extensions.Configuration;
using School.Personnel.Management.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Personnel.Management.Util
{
    public class AppConfiguration : IAppConfiguration
    {
        private readonly IConfiguration _configuration;

        public AppConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string PersonnelDbConnectionString => _configuration["ConnectionStrings:PersonnelDbConnectionString"];

    }
}
