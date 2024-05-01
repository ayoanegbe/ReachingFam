using System.ComponentModel.DataAnnotations;

namespace ReachingFam.Core.Models
{
    public class InwardItem
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
        public double TotalWeight { get; set; }
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
        [Display(Name = "Total Waste (lbs)")]
        public double? TotalWaste { get; set; }
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.Now;
        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Added By")]
        public string AddedBy { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
        public string FilePath { get; set; }
        public string ThumbnailPath { get; set; }
    }
}
