using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Enums
{
    public enum CookingItems
    {
        [Display(Name = "Cook Stove")]
        CookStove = 1,
        Oven = 2,
        [Display(Name = "Hot Plate")]
        HotPlate = 3,
        Microwave = 4,
    }
}
