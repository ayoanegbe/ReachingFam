using ReachingFam.Core.Enums;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class StockViewModel
    {
        public int StockId { get; set; }
        public int FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; }
        public decimal Quantity { get; set; }
        public DateTime Date { get; set; }
        public TransactionType TransactionType { get; set; } = TransactionType.Add;
    }
}
