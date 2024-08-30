using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class SubstituteViewModel
    {
        public int FoodItemId { get; set; }
        public string Name { get; set; }

        public List<SubstituteItem> Substitutes { get; set; }
        public List<SelectListItem> AvailableFoodItems { get; set; }
    }
}
