using AutoMapper;
using MealMate.BLL.Dtos.Stores;
using MealMate.BLL.IServices;
using MealMate.BLL.IServices.Utility;
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
        private readonly IMapProductService _mapProductAppService;
        private readonly IMapper _mapper;

        public StoreAppService(IStoreRepository storeRepository, IAtRepository atRepository, IProductRepository productRepository, IMapper mapper, IMapProductService productAppService)
        {
            _storeRepository = storeRepository;
            _atRepository = atRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _mapProductAppService = productAppService;
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
            var productDto = await _mapProductAppService.MapProductDto(product);
            await _atRepository.CreateAsync(newAt);

            var atDto = new ATDto
            {
                ProductID = productid,
                StoreID = storeid,
                NumberAtStore = amount,
                Product = productDto
            };
            return atDto;
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

        public async Task<StoreDto> GetStoreByIdAsync(Guid storeId)
        {
            var store = await _storeRepository.GetAsync(storeId) ?? throw new EntityNotFoundException("Store not found");
            return _mapper.Map<StoreDto>(store);
        }

        public async Task<ATDto> UpdateAmountAtAsync(Guid productid, Guid storeid, int amount)
        {
            var existingProduct = await _atRepository.GetAtByProductIDAndStoreIDAsync(productid, storeid);
            if (existingProduct == null)
            {
                var atDto = await CreateAtAsync(productid, storeid, amount);
                return atDto;
            }
            else
            {
                existingProduct.NumberAtStore += amount;
                await _atRepository.UpdateAsync(existingProduct);
                var productDto = await _mapProductAppService.MapProductDto(existingProduct.Product);
                return new ATDto
                {
                    ProductID = existingProduct.ProductID,
                    StoreID = existingProduct.StoreID,
                    NumberAtStore = existingProduct.NumberAtStore,
                    Product = productDto
                };
            }


        }
    }
}
