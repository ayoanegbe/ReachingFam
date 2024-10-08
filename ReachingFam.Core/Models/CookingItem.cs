using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models
{
    public class CookingItem
    {
        [Key]
        public int CookingItemId { get; set; }
        public int RequestFormId { get; set; }
        public RequestForm RequestForm { get; set; }
        public string Name { get; set; }
    }
}
