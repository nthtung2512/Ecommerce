using MailKit.Security;
using MealMate.DAL.Entities.Email;
using MimeKit;
using MimeKit.Text;

namespace MealMate.BLL.Services
{
    public class EmailNotificationService
    {
        public static async Task SendAsync(EmailSetting emailSetting, string subject, string message)
        {
            var email = new MimeMessage()
            {
                Subject = subject,
                Body = new TextPart(TextFormat.Html) { Text = message },
                From = { new MailboxAddress(emailSetting.Username, emailSetting.FromEmail) }
            };
            foreach (var recipientEmail in emailSetting.GetListRecipientEmails())
            {
                var to = new MailboxAddress(recipientEmail, recipientEmail);
                email.To.Add(to);
            }

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(
                emailSetting.Host,
                emailSetting.Port,
                SecureSocketOptions.StartTls
            );
            await client.AuthenticateAsync(emailSetting.FromEmail, emailSetting.Password);
            await client.SendAsync(email);
            await client.DisconnectAsync(true);
        }
    }

}
