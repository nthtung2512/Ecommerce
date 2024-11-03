using MealMate.DAL.Utils.EFCore;

namespace MealMate.DAL.Entities.Email
{
    public class EmailSetting(Guid id) : Entity<Guid>(id)
    {
        public required string Host { get; set; }
        public required int Port { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string FromEmail { get; set; }
        public required string RecipientEmails { get; set; }
        public bool IsEnabled { get; set; } = true;

        public string[] GetListRecipientEmails() => RecipientEmails.Split(';');
    }
}
