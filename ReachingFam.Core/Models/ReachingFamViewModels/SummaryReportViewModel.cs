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
        public double WeightIn { get; set; }
        public int Donors { get; set; }
        [Display(Name = "Family Weight")]
        public double FamilyWeight { get; set; }
        public int Families { get; set; }
        public int Seniors { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        [Display(Name = "Partner Weight")]
        public double PartnerWeight { get; set; }
        public int Partners { get; set; }
        [Display(Name = "Voltr. Weight")]
        public double VolunteerWeight { get; set; }
        [Display(Name = "Voltrs.")]
        public int Volunteers { get; set; }
        public double Hours { get; set; }
        public double Waste { get; set; }
        [Display(Name = "Weight Out")]
        public double WeightOut { get; set; }
        public string Date { get; set; }
    }
}
