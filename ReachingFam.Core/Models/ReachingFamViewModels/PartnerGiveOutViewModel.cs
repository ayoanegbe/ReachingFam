using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class PartnerGiveOutViewModel
    {
        public int PartnerGiveOutId { get; set; }
        [Display(Name = "Partner")]
        public int PartnerId { get; set; }
        public Partner Partner { get; set; }
        [Required]
        [Display(Name = "Collection Date")]
        [DataType(DataType.Date)]
        public DateTime CollectionDate { get; set; }
        [Required]
        [Display(Name = "Time")]
        [DataType(DataType.Time)]
        public DateTime CollectionTime { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        [Display(Name = "# Of Families")]
        public int NumberOfFamilies { get; set; }
        public int? Seniors { get; set; }
        public int? Adults { get; set; }
        public int? Children { get; set; }
        [Display(Name = "Non Perishables")]
        public bool NonPerishables { get; set; } = false;
        [Display(Name = "NP Weight")]
        public double? NonPerishablesWeight { get; set; }
        public bool Perishables { get; set; } = false;
        [Display(Name = "P Weight")]
        public double? PerishablesWeight { get; set; }
        public bool Frozen { get; set; } = false;
        [Display(Name = "F Weight")]
        public double? FrozenWeight { get; set; }
        [Display(Name = "Non Food")]
        public bool NonFood { get; set; } = false;
        [Display(Name = "NF Weight")]
        public double? NonFoodWeight { get; set; }
        [Display(Name = "Collected?")]
        public bool Collected { get; set; } = false;
        [Display(Name = "Date Collected")]
        public DateTime? DateCollected { get; set; }
        public IFormFile File { get; set; }
    }
}
