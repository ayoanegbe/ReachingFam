using Microsoft.EntityFrameworkCore;
using ReachingFam.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class StockTransactionViewModel
    {
        public int StockTransactionId { get; set; }
        public int StockId { get; set; }
        public Stock Stock { get; set; }
        [Precision(18, 2)]
        public decimal Quantity { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Balance { get; set; }
    }
}
