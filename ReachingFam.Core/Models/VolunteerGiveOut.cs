using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models
{
    public class VolunteerGiveOut : BaseEntity
    {
        [Key]
        public int VolunteerGiveOutId { get; set; }
        public string Email { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        [Display(Name = "Collection Date")]
        public DateTime CollectionDate { get; set; }
        [Required]
        [Precision(18, 2)]
        public decimal Weight { get; set; }
        [Display(Name = "Non Perishables")]
        public bool NonPerishables { get; set; } = false;
        [Display(Name = "NP Weight")]
        [Precision(18, 2)]
        public decimal? NonPerishablesWeight { get; set; }
        public bool Perishables { get; set; } = false;
        [Display(Name = "P Weight")]
        [Precision(18, 2)]
        public decimal? PerishablesWeight { get; set; }
        public bool Frozen { get; set; } = false;
        [Display(Name = "F Weight")]
        [Precision(18, 2)]
        public decimal? FrozenWeight { get; set; }
        [Display(Name = "Non Food")]
        public bool NonFood { get; set; } = false;
        [Display(Name = "NF Weight")]
        [Precision(18, 2)]
        public decimal? NonFoodWeight { get; set; }
        public List<VolunteerHamperItem> HamperItems { get; set; }
    }
}
