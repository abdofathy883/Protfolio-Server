using Core.Settings;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services
{
    public class EmailService : IEmailSender
    {
        private readonly IOptions<EmailSetting> emailSettings;
        public EmailService(IOptions<EmailSetting> options)
        {
            emailSettings = options;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailSettings.Value.FromEmail
                , emailSettings.Value.FromEmail),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            // Configure the SmtpClient
            using var smtpClient = new SmtpClient(emailSettings.Value.Host, emailSettings.Value.Port)
            {
                // Set credentials and enable SSL
                Credentials = new NetworkCredential(emailSettings.Value.FromEmail, emailSettings.Value.AppPassword),
                EnableSsl = true,
            };

            // Send the email
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
