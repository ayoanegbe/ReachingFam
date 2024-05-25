using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Enums
{
    public enum GenderType
    {
        Male = 1,
        Female = 2,
        [Display(Name = "Non-Binary")]
        NonBinary = 3,
        [Display(Name = "Prefer not to Answer")]
        NoAnswer = 4,
    }
}
