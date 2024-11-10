using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace MealMate.BLL.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _email;
        private readonly string _password;

        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
        {
            // Retrieve email address and app password from environment variables
            _email = Environment.GetEnvironmentVariable("EMAIL_ADDRESS");
            _password = Environment.GetEnvironmentVariable("EMAIL_APP_PASSWORD");

            // Log information about the email and password status
            logger.LogInformation("Email: {Email}", _email == null ? "Not Set" : _email.Length.ToString());
            logger.LogInformation("Password: {Password}", _password == null ? "Not Set" : _password.Length.ToString());
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                // Create and configure the SMTP client
                using (var client = new SmtpClient
                {
                    Port = 587,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_email, _password),  // Use the app password here
                    Timeout = 5000
                })
                {
                    var mailMessage = new MailMessage(_email, email, subject, htmlMessage)
                    {
                        IsBodyHtml = true
                    };
                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                // Log any exception that occurs during sending
                throw new InvalidOperationException("An error occurred while sending the email.", ex);
            }
        }
    }


}
