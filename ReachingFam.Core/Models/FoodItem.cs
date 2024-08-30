using Microsoft.EntityFrameworkCore;
using ReachingFam.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models
{
    public class FoodItem : BaseEntity
    {
        [Key]
        public int FoodItemId { get; set; }
        public string Name { get; set; }
        [Display(Name = "Item Type")]
        public FoodItemType ItemType { get; set; }
        [Display(Name = "Category")]
        public int ItemCategoryId { get; set; }
        public ItemCategory Category { get; set; }
        [Display(Name = "In Stock")]
        public bool InStock { get; set; } = false;
        [Display(Name = "Has Option")]
        public bool HasOption { get; set; } = false;
        [Display(Name = "Reorder Level")]
        [Precision(18, 2)]
        public decimal? ReorderLevel { get; set; }
        [Display(Name = "UoM")]
        public int UnitOfMeasureId { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public string Barcode { get; set; }

        public ICollection<Stock> Stocks { get; set; }
        public ICollection<FoodItemOption> Options { get; set; }
        public ICollection<FoodItemSubstitute> FoodItemSubstitutes { get; set; }
        public ICollection<FoodItemSubstitute> SubstituteForFoodItems { get; set; }
    }
}
