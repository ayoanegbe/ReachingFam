using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class FoodItemOptionViewModel
    {
        public int FoodItemOptionId { get; set; }
        [Display(Name = "Food Item")]
        public int FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; }
        public int OptionTypeId { get; set; }
        public OptionType OptionType { get; set; }
        public int OptionValueId { get; set; }
        public OptionValue OptionValue { get; set; }
    }
}
