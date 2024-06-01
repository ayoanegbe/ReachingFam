using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class HamperViewModel
    {
        public int HamperId { get; set; }
        [Display(Name = "Family")]
        public int FamilyId { get; set; }
        public Family Family { get; set; }
        [Required]
        [Display(Name = "Collection Date")]
        [DataType(DataType.Date)]
        public DateTime CollectionDate { get; set; }
        [Required]
        [Display(Name = "Time")]
        [DataType(DataType.Time)]
        public DateTime CollectionTime { get; set; }
        [Required]
        [Display(Name = "Total Weight")]
        public decimal Weight { get; set; }
        [Required]
        [Display(Name = "Family Size")]
        public int FamilySize { get; set; }
        public int? Seniors { get; set; }
        public int? Adults { get; set; }
        public int? Children { get; set; }
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
        [Display(Name = "Collected?")]
        public bool Collected { get; set; } = false;
        [Display(Name = "Date Collected")]
        public DateTime? DateCollected { get; set; }
        public IFormFile File { get; set; }
        public List<HamperItem> HamperItems { get; set; }
    }
}
