using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class VolunteerGiveOutViewModel
    {
        public int VolunteerGiveOutId { get; set; }
        [Required]
        [Display(Name = "Volunteer")]
        public string Email { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        [Display(Name = "Collection Date")]
        public DateTime CollectionDate { get; set; }
        [Required]
        public decimal Weight { get; set; }
        [Display(Name = "Non Perishables")]
        public bool NonPerishables { get; set; } = false;
        [Display(Name = "NP Weight")]
        public decimal? NonPerishablesWeight { get; set; }
        public bool Perishables { get; set; } = false;
        [Display(Name = "P Weight")]
        public decimal? PerishablesWeight { get; set; }
        public bool Frozen { get; set; } = false;
        [Display(Name = "F Weight")]
        public decimal? FrozenWeight { get; set; }
        [Display(Name = "Non Food")]
        public bool NonFood { get; set; } = false;
        [Display(Name = "NF Weight")]
        public decimal? NonFoodWeight { get; set; }
    }
}
