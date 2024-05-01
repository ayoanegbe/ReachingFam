using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models
{
    public class DailyCollection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Collection Date")]
        [DataType(DataType.Date)]
        public DateTime CollectionDate { get; set; }
        [Display(Name = "Time")]
        [DataType(DataType.Time)]
        public DateTime CollectionTime { get; set; }
        public double Weight { get; set; }
        [Display(Name = "Collected?")]
        public bool Collected { get; set; } = false;
        public string Source { get; set; }
    }
}
