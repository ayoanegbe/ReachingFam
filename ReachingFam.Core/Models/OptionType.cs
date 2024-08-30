using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models
{
    public class OptionType : BaseEntity
    {
        [Key]
        public int OptionTypeId { get; set; }
        public string Name { get; set; }
    }
}
