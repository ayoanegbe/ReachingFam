using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models.DataViewModels
{
    public class ChartDataListModel
    {
        public List<string> Date { get; set; }
        public List<decimal?> Visits { get; set; }
    }
}
