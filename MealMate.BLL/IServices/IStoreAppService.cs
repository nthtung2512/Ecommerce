﻿using MealMate.BLL.Dtos.Stores;

namespace MealMate.BLL.IServices
{
    public interface IStoreAppService
    {
        Task<ATDto> CreateAtAsync(Guid productid, Guid storeid, int amount);
        Task<ATDto> UpdateAmountAtAsync(Guid productid, Guid storeid, int amount);
        Task<List<StoreDto>> GetAllStoresAsync();
        Task<StoreDto> GetStoreByIdAsync(Guid storeId);
    }
}
