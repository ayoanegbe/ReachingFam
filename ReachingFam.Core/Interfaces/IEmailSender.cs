using ReachingFam.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Interfaces
{
    public interface IEmailSender
    {
        Task<string> SendEmailAsync(EmailRequest request, List<string> cc = null);
        Task<string> SendEmailAsync(int organizationId, EmailRequest request, List<string> cc = null);
        Task<string> SendEmailAsync(string email, string code, string message);
    }
}
