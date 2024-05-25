using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models
{
    public class HamperItem
    {
        [Key]
        public int HamperItemId { get; set; }
        public int HamperId { get; set; }
        public Hamper Hamper { get; set; }
        public int FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; }
        [Precision(18, 2)]
        public decimal Quantity { get; set; }
        public string Note { get; set; }
    }
}
