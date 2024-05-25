using ReachingFam.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class FoodItemViewModel
    {
        public int FoodItemId { get; set; }
        public string Name { get; set; }
        public FoodItemType ItemType { get; set; }
        public int ItemCategoryId { get; set; }
        public ItemCategory Category { get; set; }
        public bool InStock { get; set; } = true;
        public bool HasOption { get; set; } = false;
        public decimal ReorderLevel { get; set; }
        public int UnitOfMeasureId { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
    }
}
