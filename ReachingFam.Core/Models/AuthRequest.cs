using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models
{
    public class AuthRequest
    {
        public Guid ApiUserId { get; set; }
        public string Password { get; set; }
    }
}
