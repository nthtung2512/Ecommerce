using FluentValidation;
using MealMate.DAL.IRepositories.auth;
using MealMate.DAL.Utils;
using Microsoft.AspNetCore.Identity;

namespace MealMate.DAL.Entities.ApplicationUser
{
    public class ApplicationUser : IdentityUser<Guid>, IDeletableEntity
    {
        public ApplicationUser() : base() // Parameterless constructor
        {
            // Initialization if needed
        }
        public string FName { get; set; } = string.Empty;
        public string LName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
    }

    internal class ApplicationUserValidator : AbstractValidator<ApplicationUser>
    {
        private readonly IApplicationUserRepository _applicationUserRepository;

        public ApplicationUserValidator(IApplicationUserRepository applicationUserRepository)
        {
            _applicationUserRepository = applicationUserRepository;

            RuleFor(user => user)
                .MustAsync((user, token) => IsEmailUnique(user.Email, user.Id, token))
                .WithMessage("Email must be valid and unique");
        }

        private async Task<bool> IsEmailUnique(string email, Guid customerid, CancellationToken token)
        {
            var existingUser = await _applicationUserRepository.GetByEmailAsync(email);
            return existingUser == null || existingUser.Id == customerid;
        }
    }

}
