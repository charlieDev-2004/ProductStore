using System.Net;
using System.Net.Mail;
using Core.Interfaces;
using Core.Models.Email;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmail(Email email)
        {
            var sender = _emailSettings.UserName;
            var host = _emailSettings.Host;
            var port = _emailSettings.Port;
            var password = _emailSettings.Password;

            var smtpClient = new SmtpClient(host, port);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(sender, password);
            var message = new MailMessage(sender, email.EmailReceiver, email.Subject, email.Message);
            await smtpClient.SendMailAsync(message);
        }
    }
}