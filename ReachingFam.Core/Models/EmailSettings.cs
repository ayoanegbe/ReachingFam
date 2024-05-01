using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models
{
    public class EmailSettings
    {
        public string UserName { get; set; }
        public string Host { get; set; }
        public string HostIP { get; set; }
        public string PortNumber { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string CcEmail { get; set; }
    }
}
