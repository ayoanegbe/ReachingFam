﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models
{
    public class VolunteerGiveOut
    {
        [Key]
        public int VolunteerGiveOutId { get; set; }
        public string Email { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        [Display(Name = "Collection Date")]
        public DateTime CollectionDate { get; set; }
        [Required]
        public double Weight { get; set; }
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
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.Now;
        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { get; set; }
        [Display(Name = "Added By")]
        public string AddedBy { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
    }
}
