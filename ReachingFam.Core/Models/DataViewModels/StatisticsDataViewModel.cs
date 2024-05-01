using System;
using System.Collections.Generic;
using System.Text;

namespace ReachingFam.Core.Models.DataViewModels
{
    public class StatisticsDataViewModel
    {
        public int TotalShifts { get; set; }
        public int TotalAssignedShifts { get; set; }
        public double AverageDailyShifts { get; set; }
        public int ActiveUsers { get; set; }
        public int TodayShifts { get; set; }
        public int TodayAssignedShifts { get; set; }
        public int TotalUnassignedShifts { get; set; }
    }
}
