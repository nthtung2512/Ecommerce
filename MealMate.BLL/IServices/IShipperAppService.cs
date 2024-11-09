using MealMate.BLL.Dtos.Shipper;

namespace MealMate.BLL.IServices
{
    public interface IShipperAppService
    {
        Task<List<ShipperDto>> GetListAsync();
        Task<ShipperDto> GetByIdAsync(Guid shipperId);
        Task<ShipperDto> GetShipperByPhoneNumberAsync(string phoneno);
        Task<ShipperDto> UpdateShipperAsync(Guid shipperId, ShipperUpdateDto shipperData);
        Task<ShipperDto> UpdateShipperCapacityAsync(Guid shipperId, int capacity);
        Task DeleteShipperAsync(Guid shipperId);
    }
}
