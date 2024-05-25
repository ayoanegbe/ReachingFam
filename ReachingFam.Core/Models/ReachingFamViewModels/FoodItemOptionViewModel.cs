using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class FoodItemOptionViewModel
    {
        public int FoodItemOptionId { get; set; }
        public int FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; }
        public string Option { get; set; }
    }
}
