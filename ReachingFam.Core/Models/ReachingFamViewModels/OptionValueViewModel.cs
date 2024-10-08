using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class OptionValueViewModel
    {
        public int OptionValueId { get; set; }
        [Display(Name = "Option Type")]
        public int OptionTypeId { get; set; }
        public OptionType OptionType { get; set; }
        public string Name { get; set; }
    }
}
