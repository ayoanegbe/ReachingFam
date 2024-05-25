using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models
{
    public class ItemCategory
    {
        [Key]
        public int ItemCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
