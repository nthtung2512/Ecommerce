using Microsoft.AspNetCore.Identity.UI.Services;

namespace MealMate.BLL.Services
{
    internal class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Implement your email-sending logic here.
            // For testing, you can just log the email or print it to console.
            return Task.CompletedTask;
        }
    }
}
