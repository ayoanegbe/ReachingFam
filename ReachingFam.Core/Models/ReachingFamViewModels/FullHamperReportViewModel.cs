using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class FullHamperReportViewModel
    {
        public decimal WeightIn { get; set; }
        public decimal WeightOut { get; set; }
        public decimal TotalHamperWeight { get; set; }
        public List<DailyCollection> DailyCollections { get; set; }
    }
}
