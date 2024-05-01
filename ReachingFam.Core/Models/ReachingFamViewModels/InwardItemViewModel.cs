using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class InwardItemViewModel
    {
        public int InwardItemId { get; set; }
        [Display(Name = "Donor")]
        public int DonorId { get; set; }
        public Donor Donor { get; set; }
        [Required]
        [Display(Name = "Collection Date")]
        public DateTime CollectionDate { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N3}")]
        [Display(Name = "Total Weight (lbs)")]
        public double TotalWeight { get; set; }
        [Display(Name = "Non Perishables")]
        public bool NonPerishables { get; set; } = false;
        [Display(Name = "NP Weight")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N3}")]
        public double? NonPerishablesWeight { get; set; }
        public bool Perishables { get; set; } = false;
        [Display(Name = "P Weight")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N3}")]
        public double? PerishablesWeight { get; set; }
        public bool Frozen { get; set; } = false;
        [Display(Name = "F Weight")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N3}")]
        public double? FrozenWeight { get; set; }
        [Display(Name = "Non Food")]
        public bool NonFood { get; set; } = false;
        [Display(Name = "NF Weight")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N3}")]
        public double? NonFoodWeight { get; set; }
        [Display(Name = "Total Waste (lbs)")]
        public double? TotalWaste { get; set; }
        public IFormFile File { get; set; }
    }
}
