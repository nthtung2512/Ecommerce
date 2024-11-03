using MealMate.BLL.Dtos.Shipper;

namespace MealMate.BLL.IServices
{
    public interface IShipperAppService
    {
        Task<ShipperCreationDto> GetByIdAsync(Guid shipperId);
        Task<ShipperCreationDto> GetFreeShipperByAreaAsync(string area);
        Task<ShipperCreationDto> CreateShipperAsync(ShipperCreationDto shipperData);
        Task<ShipperCreationDto> UpdateShipperAsync(Guid shipperId, ShipperUpdateDto shipperData);
        Task DeleteShipperAsync(Guid shipperId);
    }
}
