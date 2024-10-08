using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Enums
{
    public enum Flavour
    {
        [Display(Name = "Flavoured")]
        Flavoured = 1,
        [Display(Name = "None")]
        None = 2,
        [Display(Name = "N/A")]
        NA = 3,
    }
}
