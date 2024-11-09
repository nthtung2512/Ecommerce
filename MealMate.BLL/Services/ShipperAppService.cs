using AutoMapper;
using MealMate.BLL.Dtos.Shipper;
using MealMate.BLL.IServices;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils.Exceptions;

namespace MealMate.BLL.Services
{
    internal class ShipperAppService : IShipperAppService
    {
        private readonly IShipperRepository _shipperRepository;
        private readonly IMapper _mapper;

        public ShipperAppService(IShipperRepository shipperRepository, IMapper mapper)
        {
            _shipperRepository = shipperRepository;
            _mapper = mapper;
        }

        public async Task<List<ShipperDto>> GetListAsync()
        {
            var shippers = await _shipperRepository.GetListAsync();
            if (shippers.Count == 0)
            {
                throw new EntityNotFoundException("No shippers found");
            }
            return _mapper.Map<List<ShipperDto>>(shippers);
        }

        public async Task<ShipperDto> GetByIdAsync(Guid shipperId)
        {
            var shipper = await _shipperRepository.GetAsync(shipperId) ?? throw new EntityNotFoundException("No shipper found");
            return _mapper.Map<ShipperDto>(shipper);
        }

        public async Task<ShipperDto> GetShipperByPhoneNumberAsync(string phoneno)
        {
            var shipper = await _shipperRepository.GetShipperByPhoneNumberAsync(phoneno) ?? throw new EntityNotFoundException("No shipper found");
            return _mapper.Map<ShipperDto>(shipper);
        }

        public async Task<ShipperDto> UpdateShipperAsync(Guid shipperId, ShipperUpdateDto shipperData)
        {
            var shipper = await _shipperRepository.GetAsync(shipperId) ?? throw new EntityNotFoundException("No shipper found");

            shipper.FName = shipperData.FName ?? shipper.FName;
            shipper.LName = shipperData.LName ?? shipper.LName;
            shipper.Address = shipperData.Address ?? shipper.Address;
            shipper.PhoneNumber = shipperData.SPhoneNo ?? shipper.PhoneNumber;

            await _shipperRepository.UpdateAsync(shipper);

            return _mapper.Map<ShipperDto>(shipper);
        }

        public async Task<ShipperDto> UpdateShipperCapacityAsync(Guid shipperId, int capacity)
        {
            var shipper = await _shipperRepository.GetAsync(shipperId) ?? throw new EntityNotFoundException("No shipper found");

            shipper.VehicleCapacity += capacity;

            await _shipperRepository.UpdateAsync(shipper);

            return _mapper.Map<ShipperDto>(shipper);
        }

        public async Task DeleteShipperAsync(Guid shipperId)
        {
            var shipper = await _shipperRepository.GetAsync(shipperId) ?? throw new EntityNotFoundException("No shipper found");

            await _shipperRepository.DeleteAsync(shipper);
        }
    }
}
