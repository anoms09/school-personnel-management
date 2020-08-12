using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Personnel.Management.Repositories.Miscellaneous
{
    public class CustomException : Exception
    {
        public CustomException(string errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        public string ErrorCode { get; }
    }
}
