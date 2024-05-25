using Newtonsoft.Json;
using ReachingFam.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models
{
    public class Donor : BaseEntity
    {
        [Key]
        public int DonorId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        [Display(Name = "Contact Email")]
        public string ContactEmail { get; set; }
        [Display(Name = "Contact Phone")]
        public string ContactPhone { get; set; }
    }
}
