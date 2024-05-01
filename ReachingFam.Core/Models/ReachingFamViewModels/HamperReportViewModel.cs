using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class HamperReportViewModel
    {
        [Display(Name = "Report Date")]
        public DateTime ReportDate { get; set; }
        
        public double TotalHamperWeight { get; set; }
        public int TotalFamilySize { get; set; }
        public int TotalSeniors { get; set; }
        public int TotalAdults { get; set; }
        public int TotalChildren { get; set; }
        public List<Hamper> HampersCollection { get; set; } = [];
    }
}
