using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models
{
    public class ApiUser
    {
        public Guid ApiUserId { get; set; } = new Guid();
        public string Password {  get; set; }
        public string ClientName { get; set; }
        public string ClientIP { get; set; }
        public DateTime ExpirationTime { get; set; }        
    }
}
