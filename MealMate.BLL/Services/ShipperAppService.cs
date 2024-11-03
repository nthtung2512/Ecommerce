using AutoMapper;
using FluentValidation;
using MealMate.BLL.Dtos.Shipper;
using MealMate.BLL.IServices;
using MealMate.DAL.Entities.Shippers;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils.Exceptions;
using MealMate.DAL.Utils.GuidUtil;

namespace MealMate.BLL.Services
{
    internal class ShipperAppService : IShipperAppService
    {
        private readonly IShipperRepository _shipperRepository;
        private readonly IValidator<ShipperPhoneNo> _shipperPhoneNoValidator;
        private readonly GuidGenerator _guidGenerator;
        private readonly IMapper _mapper;

        public ShipperAppService(IShipperRepository shipperRepository, IValidator<ShipperPhoneNo> shipperPhoneNoValidator, GuidGenerator guidGenerator, IMapper mapper)
        {
            _shipperRepository = shipperRepository;
            _shipperPhoneNoValidator = shipperPhoneNoValidator;
            _guidGenerator = guidGenerator;
            _mapper = mapper;
        }

        public async Task<ShipperCreationDto> CreateShipperAsync(ShipperCreationDto shipperData)
        {
            var newShipperId = _guidGenerator.Create();

            var phoneNos = shipperData.SPhoneNo.Select(phoneNo => new ShipperPhoneNo
            {
                ShipperID = newShipperId,
                PhoneNo = phoneNo
            }).ToList();

            var validationTasks = phoneNos.Select(async phone =>
            {
                return await _shipperPhoneNoValidator.ValidateAsync(phone);
            });

            var validationResults = await Task.WhenAll(validationTasks);

            var validationErrors = validationResults
                .SelectMany(result => result.Errors.Select(e => e.ErrorMessage))
                .ToList();

            if (validationErrors.Count != 0)
            {
                throw new EntityValidationException(
                    $"Validation exception when creating new shipper: {string.Join(", ", validationErrors)}"
                );
            }

            var areaShips = shipperData.Area.Select(area => new AreaShip
            {
                ShipperID = newShipperId,
                Area = area,
            }).ToList();

            var newShipper = new Shipper(newShipperId)
            {
                VehicleCapacity = shipperData.VehicleCapacity,
                Status = shipperData.Status,
                SAddress = shipperData.SAddress,
                SFName = shipperData.SFName,
                SLName = shipperData.SLName,
                ShipperPhoneNos = phoneNos,
                AreaShips = areaShips,
                IsDeleted = false
            };

            await _shipperRepository.CreateAsync(newShipper);

            return _mapper.Map<ShipperCreationDto>(newShipper);
        }

        public async Task DeleteShipperAsync(Guid shipperId)
        {
            var shipper = await _shipperRepository.GetAsync(shipperId) ?? throw new EntityNotFoundException("No shipper found");

            shipper.ShipperPhoneNos.Clear();
            shipper.AreaShips.Clear();

            await _shipperRepository.DeleteAsync(shipper);
        }

        public async Task<ShipperCreationDto> GetByIdAsync(Guid shipperId)
        {
            var shipper = await _shipperRepository.GetAsync(shipperId) ?? throw new EntityNotFoundException("No shipper found");
            return _mapper.Map<ShipperCreationDto>(shipper);
        }

        public async Task<ShipperCreationDto> GetFreeShipperByAreaAsync(string area)
        {
            var shipper = await _shipperRepository.GetFreeShipperByAreaAsync(area) ?? throw new EntityNotFoundException("No shipper found");
            return _mapper.Map<ShipperCreationDto>(shipper);
        }

        public async Task<ShipperCreationDto> UpdateShipperAsync(Guid shipperId, ShipperUpdateDto shipperData)
        {
            var shipper = await _shipperRepository.GetAsync(shipperId) ?? throw new EntityNotFoundException("No shipper found");

            shipper.VehicleCapacity = shipperData.VehicleCapacity ?? shipper.VehicleCapacity;
            shipper.Status = shipperData.Status ?? shipper.Status;
            shipper.SAddress = shipperData.SAddress ?? shipper.SAddress;
            shipper.SFName = shipperData.SFName ?? shipper.SFName;
            shipper.SLName = shipperData.SLName ?? shipper.SLName;

            if (shipperData.SArea != null)
            {
                shipper.AreaShips.Clear();
                foreach (var area in shipperData.SArea)
                {
                    shipper.AreaShips.Add(new AreaShip
                    {
                        ShipperID = shipper.Id,
                        Area = area,
                    });
                }
            }

            if (shipperData.SPhoneNo != null)
            {
                shipper.ShipperPhoneNos.Clear();
                foreach (var phone in shipperData.SPhoneNo)
                {
                    shipper.ShipperPhoneNos.Add(new ShipperPhoneNo
                    {
                        ShipperID = shipper.Id,
                        PhoneNo = phone,
                    });
                }
            }

            await _shipperRepository.UpdateAsync(shipper);

            var updatedShippers = await _shipperRepository.GetAsync(shipperId) ?? throw new EntityNotFoundException("No item found");

            return _mapper.Map<ShipperCreationDto>(updatedShippers);
        }
    }
}
