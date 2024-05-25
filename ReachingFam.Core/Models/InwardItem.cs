using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ReachingFam.Core.Models
{
    public class InwardItem : BaseEntity
    {
        [Key]
        public int InwardItemId { get; set; }
        public int DonorId { get; set; }
        public Donor Donor { get; set; }
        [Required]
        [Display(Name = "Collection Date")]
        public DateTime CollectionDate { get; set; }
        [Required]
        [Display(Name = "Total Weight (lbs)")]
        [Precision(18, 2)]
        public decimal TotalWeight { get; set; }
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
        [Display(Name = "Total Waste (lbs)")]
        [Precision(18, 2)]
        public decimal? TotalWaste { get; set; }
        
        public string FilePath { get; set; }
        public string ThumbnailPath { get; set; }
    }
}
