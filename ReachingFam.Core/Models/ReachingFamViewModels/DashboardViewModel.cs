using ReachingFam.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class DashboardViewModel
    {
        public int FamilyCount { get; set; }
        public decimal FamilyWeight { get; set;}
        public int SeniorCount { get; set; }
        public int AdultCount { get; set;}
        public int ChildrenCount { get; set;}
        public decimal TotalHoursWorked { get; set;}
        public decimal TotalWeightIn { get; set;}
        public decimal TotalWeightOut { get; set;}
        public decimal PartnerWeight { get; set;}
        public decimal VolunteerWeight { get; set;}
        public AnalyticPeriod Period { get; set;} = AnalyticPeriod.ThisMonth;
    }
    
}
