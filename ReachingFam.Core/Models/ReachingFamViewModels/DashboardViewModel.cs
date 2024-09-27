using Microsoft.EntityFrameworkCore;
using ReachingFam.Core.Enums;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class DashboardViewModel
    {
        public int FamilyCount { get; set; }
        [Precision(18, 2)]
        public decimal FamilyWeight { get; set;}
        public int SeniorCount { get; set; }
        public int AdultCount { get; set;}
        public int ChildrenCount { get; set;}
        [Precision(18, 2)]
        public decimal TotalHoursWorked { get; set;}
        [Precision(18, 2)]
        public decimal TotalWeightIn { get; set;}
        [Precision(18, 2)]
        public decimal TotalWeightOut { get; set;}
        [Precision(18, 2)]
        public decimal PartnerWeight { get; set;}
        [Precision(18, 2)]
        public decimal VolunteerWeight { get; set;}        
        public AnalyticPeriod Period { get; set;} = AnalyticPeriod.ThisMonth;
    }
    
}
