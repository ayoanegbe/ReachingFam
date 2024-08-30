using Microsoft.EntityFrameworkCore;
using ReachingFam.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class StockViewModel
    {
        public int StockId { get; set; }
        [Display(Name = "Donor")]
        public int DonorId { get; set; }
        public Donor Donor { get; set; }
        [Display(Name = "Food Item")]
        public int FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; }
        [Precision(18, 2)]
        public decimal Quantity { get; set; }
        [Display(Name = "Date Received")]
        public DateTime DateReceived { get; set; }

        public ICollection<StockTransaction> StockTransactions { get; set; }
    }
}
