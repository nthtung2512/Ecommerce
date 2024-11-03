using AutoMapper;
using MealMate.BLL.Dtos.Stores;
using MealMate.BLL.IServices;
using MealMate.DAL.Entities.Stores;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils.Exceptions;

namespace MealMate.BLL.Services
{
    internal class StoreAppService : IStoreAppService
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IAtRepository _atRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public StoreAppService(IStoreRepository storeRepository, IAtRepository atRepository, IProductRepository productRepository, IMapper mapper)
        {
            _storeRepository = storeRepository;
            _atRepository = atRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ATDto> CreateAtAsync(Guid productid, Guid storeid, int amount)
        {
            var product = await _productRepository.GetAsync(productid) ?? throw new EntityNotFoundException("Product not found");
            var store = await _storeRepository.GetAsync(storeid) ?? throw new EntityNotFoundException("Store not found");
            var newAt = new AT
            {
                ProductID = productid,
                StoreID = storeid,
                Product = product,
                Store = store,
                NumberAtStore = amount,
                IsDeleted = false
            };
            await _atRepository.CreateAsync(newAt);
            return _mapper.Map<ATDto>(newAt);
        }

        public async Task<List<StoreDto>> GetAllStoresAsync()
        {
            var stores = await _storeRepository.GetAllStoresAsync();
            if (stores.Count == 0)
            {
                throw new EntityNotFoundException("No stores found");
            }
            return _mapper.Map<List<StoreDto>>(stores);
        }

        public async Task<ATDto> GetAtByProductIDAndStoreIDAsync(Guid productId, Guid storeId)
        {
            var at = await _atRepository.GetAtByProductIDAndStoreIDAsync(productId, storeId) ?? throw new EntityNotFoundException("No product found");
            return _mapper.Map<ATDto>(at);
        }

        public async Task<List<ATDto>> GetAtByProductIDAsync(Guid productId)
        {
            var at = await _atRepository.GetAtByProductIDAsync(productId);
            if (at.Count == 0)
            {
                throw new EntityNotFoundException("No product found");
            }
            return _mapper.Map<List<ATDto>>(at);
        }

        public async Task<StoreDto> GetStoreByIdAsync(Guid storeId)
        {
            var store = await _storeRepository.GetAsync(storeId) ?? throw new EntityNotFoundException("Store not found");
            return _mapper.Map<StoreDto>(store);
        }

        public async Task<ATDto> UpdateAmountAtAsync(ATDto existingProduct)
        {
            var at = await _atRepository.GetAtByProductIDAndStoreIDAsync(existingProduct.ProductID, existingProduct.StoreID) ?? throw new EntityNotFoundException("No product found");
            at.NumberAtStore = existingProduct.NumberAtStore;
            await _atRepository.UpdateAmountAtAsync(at);
            return _mapper.Map<ATDto>(at);
        }
    }
}
