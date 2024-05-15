using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models.ReachingFamViewModels
{
    public class RejectionViewModel
    {
        public string approvalId { get; set; }
        public string rejectionReason { get; set; }
        public string returnUrl { get; set; }
    }
}
