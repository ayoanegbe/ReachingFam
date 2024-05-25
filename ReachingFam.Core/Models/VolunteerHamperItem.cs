using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models
{
    public class VolunteerHamperItem
    {
        public int VolunteerHamperItemId { get; set; }
        public int VolunteerGiveOutId { get; set; }
        public VolunteerGiveOut VolunteerHamper { get; set; }
        public int FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; }
        [Precision(18, 2)]
        public decimal Quantity { get; set; }
        public string Note { get; set; }
    }
}
