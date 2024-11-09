using AutoMapper;
using MealMate.BLL.Dtos.Promotion;
using MealMate.BLL.IServices;
using MealMate.DAL.Entities.Promotion;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils.Exceptions;
using MealMate.DAL.Utils.GuidUtil;

namespace MealMate.BLL.Services
{
    internal class BillPromotionAppService : IBillPromotionAppService
    {
        private readonly IBillPromotionRepository _billPromotionRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly GuidGenerator _guidGenerator;

        public BillPromotionAppService(IBillPromotionRepository billPromotionRepository, IMapper mapper, GuidGenerator guidGenerator, ITransactionRepository transactionRepository)
        {
            _billPromotionRepository = billPromotionRepository;
            _mapper = mapper;
            _guidGenerator = guidGenerator;
            _transactionRepository = transactionRepository;
        }

        public async Task<int> ApplyBillPromotionToBillAsync(Guid promotionId, Guid billId)
        {
            var bill = await _transactionRepository.GetAsync(billId) ?? throw new EntityNotFoundException("No bill found");
            var billPromotion = await _billPromotionRepository.GetBillPromotionByIdAsync(promotionId) ?? throw new EntityNotFoundException("No promotion found");
            var promoteBill = new PromoteBill()
            {
                TransactionId = billId,
                Bill = bill,
                PromotionId = promotionId,
                BillPromotion = billPromotion
            };
            billPromotion.PromoteBills.Add(promoteBill);
            billPromotion.PromotionChance -= 1;
            await _billPromotionRepository.UpdateAsync(billPromotion);

            return billPromotion.PromotionChance;
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
                ApplyPrice = promotionData.ApplyPrice,
                PromotionChance = promotionData.PromotionChance
            };

            await _billPromotionRepository.CreateAsync(newPromotion);

            return _mapper.Map<BillPromotionCreationDto>(newPromotion);
        }

        public async Task DeleteExpiredPromotionsAsync()
        {
            var expiredPromotions = await _billPromotionRepository.GetExpiredBillPromotions();
            foreach (var promotion in expiredPromotions)
            {
                await _billPromotionRepository.DeleteExpiredPromotionsAsync(promotion);
            }
        }

        /*public async Task DeletePromotionAsync(Guid id)
        {
            var promotion = await _billPromotionRepository.GetBillPromotionByIdAsync(id) ?? throw new EntityNotFoundException("No promotion found");
            await _billPromotionRepository.DeleteAsync(promotion);
        }*/

        public async Task<List<BillPromotionCreationDto>> GetAllBillPromotionsAsync()
        {
            var promotions = await _billPromotionRepository.GetAllBillPromotionsAsync();
            if (promotions.Count == 0)
            {
                throw new EntityNotFoundException("No promotion found");
            }
            return _mapper.Map<List<BillPromotionCreationDto>>(promotions);
        }

        public async Task<BillPromotionCreationDto> GetBestBillPromotionByPriceAsync(decimal totalprice)
        {
            var promotion = await _billPromotionRepository.GetBestBillPromotionByPriceAsync(totalprice) ?? throw new EntityNotFoundException("No promotion found");
            return _mapper.Map<BillPromotionCreationDto>(promotion);
        }

        public async Task<BillPromotionCreationDto> GetBillPromotionByIdAsync(Guid id)
        {
            var promotion = await _billPromotionRepository.GetBillPromotionByIdAsync(id) ?? throw new EntityNotFoundException("No promotion found");
            return _mapper.Map<BillPromotionCreationDto>(promotion);
        }

        public async Task<List<BillPromotionCreationDto>> GetPromotionsByBillId(Guid transactionId)
        {
            var promotions = await _billPromotionRepository.GetPromotionByBillId(transactionId);
            if (promotions.Count == 0)
            {
                throw new EntityNotFoundException("No promotion found for this bill");
            }
            return _mapper.Map<List<BillPromotionCreationDto>>(promotions);
        }
    }
}
