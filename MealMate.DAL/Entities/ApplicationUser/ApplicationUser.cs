using FluentValidation;
using MealMate.DAL.IRepositories.auth;
using MealMate.DAL.Utils;
using MealMate.DAL.Utils.EFCore;

namespace MealMate.DAL.Entities.ApplicationUser
{
    public class ApplicationUser(Guid id) : Entity<Guid>(id), IDeletableEntity
    {
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string Address { get; set; } = string.Empty;
        public required string PhoneNumber { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    internal class ApplicationUserValidator : AbstractValidator<ApplicationUser>
    {
        private readonly IApplicationUserRepository _applicationUserRepository;

        public ApplicationUserValidator(IApplicationUserRepository applicationUserRepository)
        {
            _applicationUserRepository = applicationUserRepository;

            RuleFor(user => user.Email)
                .MustAsync(IsEmailUnique)
                .WithMessage("Email must be valid and unique");
        }

        private async Task<bool> IsEmailUnique(string email, CancellationToken token)
        {
            return await _applicationUserRepository.GetByEmailAsync(email) == null;
        }
    }

}
