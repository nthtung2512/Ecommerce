using AutoMapper;
using MealMate.BLL.Dtos.Promotion;
using MealMate.BLL.IServices;
using MealMate.DAL.Entities.Promotion;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils.GuidUtil;

namespace MealMate.BLL.Services
{
    internal class BillPromotionAppService : IBillPromotionAppService
    {
        private readonly IBillPromotionRepository _billPromotionRepository;
        private readonly IMapper _mapper;
        private readonly GuidGenerator _guidGenerator;

        public BillPromotionAppService(IBillPromotionRepository billPromotionRepository, IMapper mapper, GuidGenerator guidGenerator)
        {
            _billPromotionRepository = billPromotionRepository;
            _mapper = mapper;
            _guidGenerator = guidGenerator;
        }

        public async Task<BillPromotionCreationDto> CreateBillPromotionAsync(BillPromotionCreationDto promotionData)
        {
            var newPromotion = new BillPromotion(_guidGenerator.Create())
            {
                Description = promotionData.Description,
                Discount = promotionData.Discount,
                Name = promotionData.Name,
                StartDay = promotionData.StartDay,
                EndDay = promotionData.EndDay,
                ApplyPrice = promotionData.ApplyPrice
            };

            await _billPromotionRepository.CreateAsync(newPromotion);

            return _mapper.Map<BillPromotionCreationDto>(newPromotion);
        }

        public async Task DeletePromotionAsync(Guid id)
        {
            var promotion = await _billPromotionRepository.GetBillPromotionByIdAsync(id) ?? throw new EntryPointNotFoundException("No promotion found");
            await _billPromotionRepository.DeleteAsync(promotion);
        }

        public async Task<List<BillPromotionCreationDto>> GetAllBillPromotionsAsync()
        {
            var promotions = await _billPromotionRepository.GetAllBillPromotionsAsync();
            if (promotions.Count == 0)
            {
                throw new EntryPointNotFoundException("No promotion found");
            }
            return _mapper.Map<List<BillPromotionCreationDto>>(promotions);
        }

        public async Task<BillPromotionCreationDto> GetBillPromotionByIdAsync(Guid id)
        {
            var promotion = await _billPromotionRepository.GetBillPromotionByIdAsync(id) ?? throw new EntryPointNotFoundException("No promotion found");
            return _mapper.Map<BillPromotionCreationDto>(promotion);
        }

        public async Task<List<BillPromotionCreationDto>> GetPromotionsByBillId(Guid transactionId)
        {
            var promotions = await _billPromotionRepository.GetPromotionByBillId(transactionId);
            if (promotions.Count == 0)
            {
                throw new EntryPointNotFoundException("No promotion found for this bill");
            }
            return _mapper.Map<List<BillPromotionCreationDto>>(promotions);
        }
    }
}
