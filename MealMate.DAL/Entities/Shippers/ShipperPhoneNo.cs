using FluentValidation;
using MealMate.DAL.IRepositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealMate.DAL.Entities.Shippers
{
    public class ShipperPhoneNo
    {
        [Key, Column(Order = 0)]
        public Guid ShipperID { get; set; }
        [Key, Column(Order = 1)]
        public required string PhoneNo { get; set; }
    }

    internal class ShipperPhoneNoValidator : AbstractValidator<ShipperPhoneNo>
    {
        private readonly IShipperRepository _shipperRepository;

        public ShipperPhoneNoValidator(IShipperRepository shipperRepository)
        {
            _shipperRepository = shipperRepository;

            RuleFor(shipperPhoneNo => shipperPhoneNo.PhoneNo)
                .MustAsync(IsPhoneNoUnique)
                .WithMessage("Phone number must be unique");
        }

        private async Task<bool> IsPhoneNoUnique(string phoneNo, CancellationToken token)
        {
            return await _shipperRepository.GetPhoneNoAsync(phoneNo) == null;
        }
    }
}
