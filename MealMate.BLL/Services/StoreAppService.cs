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
        private readonly IProductAppService _productAppService;
        private readonly IMapper _mapper;

        public StoreAppService(IStoreRepository storeRepository, IAtRepository atRepository, IProductRepository productRepository, IMapper mapper, IProductAppService productAppService)
        {
            _storeRepository = storeRepository;
            _atRepository = atRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _productAppService = productAppService;
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
            var productDto = await _productAppService.MapProductDto(product);
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

        public async Task<ATDto> GetAtByProductIDAndStoreIDAsync(Guid productId, Guid storeId)
        {
            var at = await _atRepository.GetAtByProductIDAndStoreIDAsync(productId, storeId) ?? throw new EntityNotFoundException("No product found");
            var product = await _productRepository.GetAsync(productId) ?? throw new EntityNotFoundException("No product found");
            var productDto = await _productAppService.MapProductDto(product);
            return new ATDto
            {
                ProductID = productId,
                StoreID = storeId,
                NumberAtStore = at.NumberAtStore,
                Product = productDto
            };
        }

        public async Task<List<ATDto>> GetAtByProductIDAsync(Guid productId)
        {
            var at = await _atRepository.GetAtByProductIDAsync(productId);
            if (at.Count == 0)
            {
                throw new EntityNotFoundException("No product found");
            }
            var atDtos = new List<ATDto>();
            foreach (var item in at)
            {
                var product = await _productRepository.GetAsync(productId) ?? throw new EntityNotFoundException("No product found");
                var productDto = await _productAppService.MapProductDto(product);
                atDtos.Add(new ATDto
                {
                    ProductID = productId,
                    StoreID = item.StoreID,
                    NumberAtStore = item.NumberAtStore,
                    Product = productDto
                });
            }
            return atDtos;
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
                var productDto = await _productAppService.MapProductDto(existingProduct.Product);
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
