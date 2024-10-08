using ReachingFam.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Models
{
    public class AuthResponse
    {
        public ResponseStatus Status { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}
