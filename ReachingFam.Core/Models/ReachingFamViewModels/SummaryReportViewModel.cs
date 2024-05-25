using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class SummaryReportViewModel
    {
        [Display(Name = "Weight In")]
        public decimal WeightIn { get; set; }
        public int Donors { get; set; }
        [Display(Name = "Family Weight")]
        public decimal FamilyWeight { get; set; }
        public int Families { get; set; }
        public int Seniors { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        [Display(Name = "Partner Weight")]
        public decimal PartnerWeight { get; set; }
        public int Partners { get; set; }
        [Display(Name = "Voltr. Weight")]
        public decimal VolunteerWeight { get; set; }
        [Display(Name = "Voltrs.")]
        public int Volunteers { get; set; }
        public decimal Hours { get; set; }
        public decimal Waste { get; set; }
        [Display(Name = "Weight Out")]
        public decimal WeightOut { get; set; }
        public string Date { get; set; }
    }
}
