using System.Net;
using System.Net.Mail;
using Narratify.Services.Interfaces;

namespace Narratify.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string message)
        {
            var host = _configuration["Smtp:Host"] ?? throw new ArgumentNullException(nameof(_configuration), "Smtp:Host is not configured.");
            var port = _configuration["Smtp:Port"] ?? throw new ArgumentNullException(nameof(_configuration), "Smtp:Port is not configured.");
            var username = _configuration["Smtp:Username"] ?? throw new ArgumentNullException(nameof(_configuration), "Smtp:Username is not configured.");
            var password = _configuration["Smtp:Password"] ?? throw new ArgumentNullException(nameof(_configuration), "Smtp:Password is not configured.");
            var from = _configuration["Smtp:From"] ?? throw new ArgumentNullException(nameof(_configuration), "Smtp:From is not configured.");
            var enableSsl = _configuration["Smtp:EnableSsl"] ?? throw new ArgumentNullException(nameof(_configuration), "Smtp:EnableSsl is not configured.");

            var smtpClient = new SmtpClient(host)
            {
                Port = int.Parse(port),
                Credentials = new NetworkCredential(username, password),
                EnableSsl = bool.Parse(enableSsl),
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(from),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}