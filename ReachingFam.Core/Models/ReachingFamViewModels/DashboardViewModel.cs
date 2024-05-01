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
        public double FamilyWeight { get; set;}
        public int SeniorCount { get; set; }
        public int AdultCount { get; set;}
        public int ChildrenCount { get; set;}
        public double TotalHoursWorked { get; set;}
        public double TotalWeightIn { get; set;}
        public double TotalWeightOut { get; set;}
        public double PartnerWeight { get; set;}
        public double VolunteerWeight { get; set;}
        public AnalyticPeriod Period { get; set;} = AnalyticPeriod.ThisMonth;
    }
    
}
