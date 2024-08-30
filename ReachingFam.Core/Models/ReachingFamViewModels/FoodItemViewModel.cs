using ReachingFam.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class FoodItemViewModel
    {
        public int FoodItemId { get; set; }
        public string Name { get; set; }
        [Display(Name = "Item Type")]
        public FoodItemType ItemType { get; set; }
        [Display(Name = "Category")]
        public int ItemCategoryId { get; set; }
        public ItemCategory Category { get; set; }
        [Display(Name = "Option")]
        public int FoodItemOptionId { get; set; }
        public FoodItemOption FoodItemOption { get; set; }
        [Display(Name = "Substitute")]
        public int FoodItemSubstituteId { get; set; }
        public FoodItemSubstitute Substitute { get; set; }
        [Display(Name = "In Stock")]
        public bool InStock { get; set; } = false;
        [Display(Name = "Has Option")]
        public bool HasOption { get; set; } = false;
        [Display(Name = "Reorder Level")]
        public decimal ReorderLevel { get; set; }
        [Display(Name = "UoM")]
        public int UnitOfMeasureId { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public string Barcode { get; set; }
    }
}
