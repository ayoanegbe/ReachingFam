using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models
{
    public class Location : BaseEntity
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
