using Microsoft.EntityFrameworkCore;
using ReachingFam.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class CurrentPreviousDashboardViewModel
    {
        public DashboardViewModel Current { get; set; } = new DashboardViewModel();
        public DashboardViewModel Previous { get; set; } = new DashboardViewModel();
        [Precision(18, 2)]
        public decimal FamilyCountPercentage { get; set; }
        [Precision(18, 2)]
        public decimal FamilyWeightPercentage { get; set; }
        [Precision(18, 2)]
        public decimal SeniorCountPercentage { get; set; }
        [Precision(18, 2)]
        public decimal AdultCountPercentage { get; set; }
        [Precision(18, 2)]
        public decimal ChildrenCountPercentage { get; set; }
        [Precision(18, 2)]
        public decimal TotalHoursWorkedPercentage { get; set; }
        [Precision(18, 2)]
        public decimal TotalWeightInPercentage { get; set; }
        [Precision(18, 2)]
        public decimal TotalWeigthOutPercentage { get; set; }
        [Precision(18, 2)]
        public decimal PartnerWeightPercentage { get; set; }
        [Precision(18, 2)]
        public decimal VolunteerWeightPercentage { get; set; }
        [Precision(18, 2)]
        public decimal TotalWeightDistributed { get; set; }
        [Precision(18, 2)]
        public decimal TotalEmergencyHampers { get; set; }
        public AnalyticPeriod Period { get; set; }
        public List<FoodItem> FooditemsBelowReorderLevel { get; set; } = [];
        public long FamilyHampersForCollection { get; set; }
        public long FamilyHampersCollected { get; set; }
        public long FamilyHampersNotCollected { get; set; }
        public long PartnerHampersForCollection { get; set; }
        public long PartnerHampersCollected { get; set ; }
        public long PartnerHampersNotCollected { get; set; }
        public GraphViewModel GraphView { get; set; } = new GraphViewModel();
    }
}
