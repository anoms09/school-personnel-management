using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Personnel.Management.Repositories.Miscellaneous
{
    public static class HelpMe
    {
        public static string StringifyValidationErrors(ModelStateDictionary modelState)
        {
            return
                string.Join(" | ", modelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
        }
    }
}
