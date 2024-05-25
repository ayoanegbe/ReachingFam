using Microsoft.EntityFrameworkCore;
using ReachingFam.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models
{
    public class Hamper : BaseEntity
    {
        [Key]
        public int HamperId { get; set; }
        public int FamilyId { get; set; }
        public Family Family { get; set; }
        public bool AgreeToOneHamper { get; set; }
        [Required]
        [Display(Name = "Collection Date")]
        [DataType(DataType.Date)]
        public DateTime CollectionDate { get; set; }
        [Required]
        [Display(Name = "Time")]
        [DataType(DataType.Time)]
        public DateTime CollectionTime { get; set; }
        [Required]
        [Precision(18, 2)]
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
        [Display(Name = "Collected?")]
        public bool Collected { get; set; } = false;
        [Display(Name = "Date Collected")]
        public DateTime? DateCollected { get; set; }
        public List<HamperItem> HamperItems { get; set; }
        public string FilePath { get; set; }
        public string ThumbnailPath { get; set; }
    }
}
