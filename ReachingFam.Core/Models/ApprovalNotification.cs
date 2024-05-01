using ReachingFam.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models
{
    public class ApprovalNotification
    {
        [Key]
        public int NotificationId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime Date { get; set; } = DateTime.Now;
        public DateTime ReadDate { get; set; }

    }
}
