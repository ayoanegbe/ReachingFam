using ReachingFam.Core.Models;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using ReachingFam.Core.Data;
using ReachingFam.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ReachingFam.Core.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public EmailSender(ApplicationDbContext context, ILogger<EmailSender> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<string> SendEmailAsync(EmailRequest request, List<string> cc = null)
        {
            var smtp = await _context.SmtpSettings.FirstOrDefaultAsync();

            var result = string.Empty;

            try
            {
                using SmtpClient client = new();
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                MailMessage mail = new(smtp.HostIP, request.RecieverEmailAddress);
                client.Port = int.Parse(smtp.PortNumber);
                client.Host = smtp.Host;
                client.Credentials = new NetworkCredential(smtp.UserName, smtp.Password);
                client.EnableSsl = true;
                mail.From = new MailAddress(smtp.HostIP, smtp.DisplayName);
                mail.Subject = request.Subject;
                mail.IsBodyHtml = true;

                if (cc != null && cc.Count > 0)
                {
                    foreach (var item in cc)
                    {
                        if (item != null)
                        {
                            mail.CC.Add(item);
                        }
                    }
                }

                mail.CC.Add(smtp.CcEmail);
                //mail.To.Add(new MailAddress(request.RecieverEmailAddress));
                mail.Sender = new MailAddress(smtp.HostIP, smtp.DisplayName);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                var bodyMessage = request.Body;

                mail.Body = bodyMessage;

                client.Send(mail);

                client.Dispose();

                result = "success";

            }
            catch (Exception ex)
            {
                result = ex.ToString();
                _logger.Log(LogLevel.Debug, $"Error sending email {result}");
            }

            return result;
        }

        public async Task<string> SendEmailAsync(int organizationId, EmailRequest request, List<string> cc = null)
        {
            var smtp = await _context.SmtpSettings.FirstOrDefaultAsync();

            var result = string.Empty;

            try
            {
                using SmtpClient client = new();
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                MailMessage mail = new(smtp.HostIP, request.RecieverEmailAddress);
                client.Port = int.Parse(smtp.PortNumber);
                client.Host = smtp.Host;
                client.Credentials = new NetworkCredential(smtp.UserName, smtp.Password);
                client.EnableSsl = true;
                mail.From = new MailAddress(smtp.HostIP, smtp.DisplayName);
                mail.Subject = request.Subject;
                mail.IsBodyHtml = true;

                if (cc != null && cc.Count > 0)
                {
                    foreach (var item in cc)
                    {
                        if (item != null)
                        {
                            mail.CC.Add(item);
                        }
                    }
                }

                mail.CC.Add(smtp.CcEmail);
                //mail.To.Add(new MailAddress(request.RecieverEmailAddress));
                mail.Sender = new MailAddress(smtp.HostIP, smtp.DisplayName);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                var bodyMessage = request.Body;

                mail.Body = bodyMessage;

                client.Send(mail);

                client.Dispose();

                result = "success";

            }
            catch (Exception ex)
            {
                result = ex.ToString();
                _logger.Log(LogLevel.Debug, $"Error sending email {result}");
            }

            return result;
        }

        public Task<string> SendEmailAsync(string email, string code, string message)
        {
            throw new NotImplementedException();
        }
    }
}
