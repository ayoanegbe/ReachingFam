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
    public class StockHistory
    {
        [Key]
        public int StockHistoryId { get; set; }
        public int FoodItemId { get; set; }
        public virtual FoodItem FoodItem { get; set; }
        [Precision(18, 2)]
        public decimal Quantity { get; set; }
        [Display(Name = "Adjusted Quantity")]
        [Precision(18, 2)]
        public decimal AdjustedQuantity { get; set; }
        [Display(Name = "Transaction Type")]
        public TransactionType TransactionType { get; set; }
        public DateTime Date { get; set; }
    }
}
