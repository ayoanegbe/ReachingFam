using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Enums
{
    public enum DisabilityClass
    {
        Disability = 1,
        [Display(Name = "Invisible Illness")]
        InvisibleIllness = 2,
        [Display(Name = "N/A")]
        NotApplicable = 3,
    }
}
