using ReachingFam.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models
{
    public class AuditTrail
    {
        [Key]
        public int AuditTrailId { get; set; } 
        public string TableName { get; set; }
        public UpdateAction Action { get; set; } = UpdateAction.Update;
        public string OldValue { get; set; } = string.Empty;
        public string NewValue { get; set; }
        public DateTime ChangeDate { get; set; } = DateTime.UtcNow;
        public string ChangedBy { get; set; }
    }
}
