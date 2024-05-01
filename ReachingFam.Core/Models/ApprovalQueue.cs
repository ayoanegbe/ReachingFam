using ReachingFam.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models
{
    public class ApprovalQueue
    {
        [Key]
        public int ApprovalQueueId { get; set; }
        public string TableName { get; set; }
        public string ModuleName { get; set; }
        public UpdateAction Action { get; set; } = UpdateAction.Update;
        public string OldValue { get; set; } = string.Empty;
        public string NewValue { get; set; }
        public DateTime ChangeDate { get; set; } = DateTime.Now;
        public string ChangedBy { get; set; }
        public ApprovalStatus Status { get; set; } = ApprovalStatus.Pending;
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string RejectionReason { get; set; }
        public string RejectedBy { get; set; }
        public DateTime? RejectionDate { get; set; }
    }
}
