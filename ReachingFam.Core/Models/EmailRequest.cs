using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models
{
    public class EmailRequest
    {
        public string SenderEmailAddress { get; set; }
        public string RecieverEmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
