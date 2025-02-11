using GotExplorer.BLL.Options;
using GotExplorer.BLL.Services.Interfaces;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace GotExplorer.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpOptions _smtpOptions;

        public EmailService(IOptions<SmtpOptions> smtpOptions)
        {
            _smtpOptions = smtpOptions.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, SecureSocketOptions.Auto);
                await smtpClient.AuthenticateAsync(_smtpOptions.Username, _smtpOptions.Password);

                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(_smtpOptions.FromName, _smtpOptions.From));
                message.To.Add(MailboxAddress.Parse(toEmail));
                message.Subject = subject;
                var bodyBuilder = new BodyBuilder() 
                { 
                    HtmlBody = body 
                };
                message.Body = bodyBuilder.ToMessageBody();

                await smtpClient.SendAsync(message);
                await smtpClient.DisconnectAsync(true);
            }
        }
    }
}
