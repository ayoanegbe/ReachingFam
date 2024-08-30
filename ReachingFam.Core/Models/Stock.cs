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
    public class Stock : BaseEntity
    {
        [Key]
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
