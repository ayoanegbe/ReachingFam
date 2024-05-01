using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Enums
{
    public enum AnalyticPeriod
    {
        [Display(Name = "This Month")]
        ThisMonth = 1,
        [Display(Name = "This Year")]
        ThisYear = 2,
        [Display(Name = "This Week")]
        ThisWeek = 3,
        [Display(Name = "Last Month")]
        LastMonth = 4
    }
}
