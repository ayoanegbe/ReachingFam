using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class DonorViewModel
    {
        public int DonorId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Contact Email")]
        public string ContactEmail { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Contact Phone")]
        public string ContactPhone { get; set; }
    }
}
