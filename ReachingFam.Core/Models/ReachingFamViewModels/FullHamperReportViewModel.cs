using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class FullHamperReportViewModel
    {
        public double WeightIn { get; set; }
        public double WeightOut { get; set; }
        public double TotalHamperWeight { get; set; }
        public List<DailyCollection> DailyCollections { get; set; }
    }
}
