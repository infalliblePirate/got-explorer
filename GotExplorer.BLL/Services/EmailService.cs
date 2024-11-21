using GotExplorer.BLL.Options;
using GotExplorer.BLL.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace GotExplorer.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpOptions _smtpOptions;

        public EmailService(IOptions<SmtpOptions> smtpOptions)
        {
            _smtpOptions = smtpOptions.Value;
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            using (var smtpClient = new SmtpClient(_smtpOptions.Host,_smtpOptions.Port))
            {
                smtpClient.EnableSsl = _smtpOptions.EnableSSL;
                smtpClient.UseDefaultCredentials = _smtpOptions.UseDefaultCredentials;
                smtpClient.Credentials = new NetworkCredential(_smtpOptions.Username, _smtpOptions.Password);

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_smtpOptions.From);
                mailMessage.To.Add(toEmail);
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = body;

                smtpClient.Send(mailMessage);
            }
        }
    }
}
