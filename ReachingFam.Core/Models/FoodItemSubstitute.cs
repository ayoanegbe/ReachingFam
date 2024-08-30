using System.ComponentModel.DataAnnotations;

namespace ReachingFam.Core.Models
{
    public class FoodItemSubstitute : BaseEntity
    {
        [Key]
        public int FoodItemSubstituteId { get; set; }
        public int SubstituteFoodItemId { get; set; }
        public int FoodItemId { get; set; }

        public FoodItem FoodItem { get; set; }        
        public FoodItem SubstituteFoodItem { get; set; }        
    }
}
