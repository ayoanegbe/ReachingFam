using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class FoodItemSubstituteViewModel
    {
        public int FoodItemSubstituteId { get; set; }
        public FoodItem SubstituteFoodItem { get; set; }
        public int SubstituteFoodItemId { get; set; }
        public int FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; }
        
    }
}
