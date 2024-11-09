using MealMate.BLL.Dtos.Product;
using MealMate.BLL.Dtos.Promotion;
using MealMate.BLL.IServices;
using MealMate.DAL.Entities.Promotion;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils.Exceptions;
using MealMate.DAL.Utils.GuidUtil;

namespace MealMate.BLL.Services
{
    internal class CustomerPromotionAppService : ICustomerPromotionAppService
    {
        private readonly ICustomerPromotionRepository _customerPromotionRepository;
        private readonly IProductRepository _productRepository;
        private readonly GuidGenerator _guidGenerator;

        public CustomerPromotionAppService(ICustomerPromotionRepository customerPromotionRepository, GuidGenerator guidGenerator, IProductRepository productRepository)
        {
            _customerPromotionRepository = customerPromotionRepository;
            _guidGenerator = guidGenerator;
            _productRepository = productRepository;
        }

        public async Task AssignPromotionToCustomerAsync(Guid promotionId, Guid customerid)
        {
            var promotion = await _customerPromotionRepository.GetCustomerPromotionByIdAsync(promotionId) ?? throw new EntityNotFoundException("No promotion found");
            var promoteCustomer = new PromoteCustomer()
            {
                CustomerId = customerid,
                CustomerPromotion = promotion,
                PromotionId = promotionId
            };
            promotion.PromoteCustomers.Add(promoteCustomer);
            await _customerPromotionRepository.UpdateAsync(promotion);
        }

        /*  public decimal CalculateCustomerDiscount(decimal olddiscount, decimal discount)
          {
              return Math.Min(olddiscount + discount, 0.99m);
          }*/

        public async Task<CustomerPromotionDto> CreateCustomerPromotionAsync(CustomerPromotionCreationDto customerPromotionCreationDto)
        {
            var promotion = new CustomerPromotion(_guidGenerator.Create())
            {
                Discount = customerPromotionCreationDto.Discount,
                Name = customerPromotionCreationDto.Name,
                Description = customerPromotionCreationDto.Description,
                StartDay = customerPromotionCreationDto.StartDay,
                EndDay = customerPromotionCreationDto.EndDay,
                ProductId = customerPromotionCreationDto.ProductId
            };
            await _customerPromotionRepository.CreateAsync(promotion);
            var product = await _productRepository.GetAsync(promotion.ProductId) ?? throw new EntityNotFoundException("No product found");
            return new CustomerPromotionDto
            {
                PromotionId = promotion.Id,
                Discount = promotion.Discount,
                Name = promotion.Name,
                Description = promotion.Description,
                StartDay = promotion.StartDay,
                EndDay = promotion.EndDay,
                Product = new ProductCreationDto
                {
                    ProductId = promotion.ProductId,
                    PName = product.PName,
                    Description = product.Description,
                    Price = product.Price,
                    Category = product.Category,
                    Weight = product.Weight,
                    ImageURL = product.ImageURL,
                }
            };
        }

        public async Task DeleteCustomerPromotionAsync(Guid promotionid, Guid customerid)
        {
            var promoteCustomer = await _customerPromotionRepository.GetCustomerPromotionByPCIdAsync(promotionid, customerid) ?? throw new EntityNotFoundException("No customer promotion found");
            var promotion = await _customerPromotionRepository.GetCustomerPromotionByIdAsync(promotionid) ?? throw new EntityNotFoundException("No customer promotion found");
            promoteCustomer.CustomerPromotion = null;
            await _customerPromotionRepository.DeletePromoteCustomerAsync(promoteCustomer);
        }

        public async Task DeleteExpiredPromotionsAsync()
        {
            var expiredPromotions = await _customerPromotionRepository.GetExpiredCustomerPromotions();
            foreach (var promotion in expiredPromotions)
            {
                await _customerPromotionRepository.DeleteAsync(promotion);
            }
        }

        public async Task<CustomerPromotionDto> GetCustomerPromotionByIdAsync(Guid promotionid)
        {
            var promotion = await _customerPromotionRepository.GetCustomerPromotionByIdAsync(promotionid) ?? throw new EntityNotFoundException("No customer promotion found");
            var product = await _productRepository.GetAsync(promotion.ProductId) ?? throw new EntityNotFoundException("No product found");
            return new CustomerPromotionDto
            {
                PromotionId = promotion.Id,
                Discount = promotion.Discount,
                Name = promotion.Name,
                Description = promotion.Description,
                StartDay = promotion.StartDay,
                EndDay = promotion.EndDay,
                Product = new ProductCreationDto
                {
                    ProductId = promotion.ProductId,
                    PName = product.PName,
                    Description = product.Description,
                    Price = product.Price,
                    Category = product.Category,
                    Weight = product.Weight,
                    ImageURL = product.ImageURL,
                }
            };
        }

        public async Task<List<CustomerPromotionDto>> GetDiscountByProductIdListAsync(List<Guid> productIdList)
        {
            var promotions = await _customerPromotionRepository.GetDiscountByProductIdListAsync(productIdList);
            if (promotions.Count == 0)
            {
                throw new EntityNotFoundException("No customer promotions found");
            }
            var promotionDtos = new List<CustomerPromotionDto>();
            foreach (var promotion in promotions)
            {
                var product = await _productRepository.GetAsync(promotion.ProductId) ?? throw new EntityNotFoundException("No product found");
                promotionDtos.Add(new CustomerPromotionDto
                {
                    PromotionId = promotion.Id,
                    Discount = promotion.Discount,
                    Name = promotion.Name,
                    Description = promotion.Description,
                    StartDay = promotion.StartDay,
                    EndDay = promotion.EndDay,
                    Product = new ProductCreationDto
                    {
                        ProductId = promotion.ProductId,
                        PName = product.PName,
                        Description = product.Description,
                        Price = product.Price,
                        Category = product.Category,
                        Weight = product.Weight,
                        ImageURL = product.ImageURL,
                    }
                });
            }
            return promotionDtos;
        }

        public async Task<List<CustomerPromotionDto>> GetListAsync()
        {
            var promotions = await _customerPromotionRepository.GetListAsync();
            if (promotions.Count == 0)
            {
                throw new EntityNotFoundException("No customer promotions found");
            }
            var promotionDtos = new List<CustomerPromotionDto>();
            foreach (var promotion in promotions)
            {
                var product = await _productRepository.GetAsync(promotion.ProductId) ?? throw new EntityNotFoundException("No product found");
                promotionDtos.Add(new CustomerPromotionDto
                {
                    PromotionId = promotion.Id,
                    Discount = promotion.Discount,
                    Name = promotion.Name,
                    Description = promotion.Description,
                    StartDay = promotion.StartDay,
                    EndDay = promotion.EndDay,
                    Product = new ProductCreationDto
                    {
                        ProductId = promotion.ProductId,
                        PName = product.PName,
                        Description = product.Description,
                        Price = product.Price,
                        Category = product.Category,
                        Weight = product.Weight,
                        ImageURL = product.ImageURL,
                    }
                });
            }
            return promotionDtos;
        }

        public async Task<List<CustomerPromotionDto>> GetListByCustomerIdAsync(Guid customerId)
        {
            var promotions = await _customerPromotionRepository.GetListByCustomerIdAsync(customerId);
            if (promotions.Count == 0)
            {
                throw new EntityNotFoundException("No customer promotions found");
            }
            var promotionDtos = new List<CustomerPromotionDto>();
            foreach (var promotion in promotions)
            {
                var product = await _productRepository.GetAsync(promotion.ProductId) ?? throw new EntityNotFoundException("No product found");
                promotionDtos.Add(new CustomerPromotionDto
                {
                    PromotionId = promotion.Id,
                    Discount = promotion.Discount,
                    Name = promotion.Name,
                    Description = promotion.Description,
                    StartDay = promotion.StartDay,
                    EndDay = promotion.EndDay,
                    Product = new ProductCreationDto
                    {
                        ProductId = promotion.ProductId,
                        PName = product.PName,
                        Description = product.Description,
                        Price = product.Price,
                        Category = product.Category,
                        Weight = product.Weight,
                        ImageURL = product.ImageURL,
                    },
                    CustomerId = customerId
                });
            }
            return promotionDtos;
        }
    }
}
