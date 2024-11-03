using AutoMapper;
using MealMate.BLL.Dtos.Bills;
using MealMate.BLL.IServices;
using MealMate.DAL.Entities.Products;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils.Exceptions;
using MealMate.DAL.Utils.GuidUtil;

namespace MealMate.BLL.Services
{
    internal class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IProductRepository _productRepository;
        private readonly GuidGenerator _guidGenerator;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, GuidGenerator guidGenerator, IMapper mapper, IProductRepository productRepository)
        {
            _transactionRepository = transactionRepository;
            _guidGenerator = guidGenerator;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        // Implement later
        public Task<BillCreationDto> AssignShipperAsync(BillCreationDto assignedBill)
        {
            throw new NotImplementedException();
        }

        public async Task<BillCreationDto> CreateBillAsync(BillCreationDto billData)
        {
            var newId = _guidGenerator.Create();
            var newBill = new Bill(newId)
            {
                PaymentMethod = billData.PaymentMethod,
                DateAndTime = billData.DateAndTime,
                CustomerID = billData.CustomerID,
                IsShipped = billData.IsShipped,
                ShipperID = billData.ShipperID,
                StoreID = billData.StoreID,
                IsDeleted = false
            };

            var includes = await Task.WhenAll(billData.Includes.Select(async includeDto => new Include
            {
                TransactionID = newId,
                ProductID = includeDto.ProductID,
                Product = await _productRepository.GetAsync(includeDto.ProductID)
                  ?? throw new EntityNotFoundException("Product not found"),
                Transaction = newBill,
                NumberOfProductInBill = includeDto.NumberOfProductInBill,
                SubTotal = includeDto.SubTotal,
                IsDeleted = false
            }));

            newBill.Includes.AddRange(includes);

            await _transactionRepository.CreateAsync(newBill);
            return _mapper.Map<BillCreationDto>(newBill);
        }

        public async Task<BillCreationDto> GetBillByIdAsync(Guid transactionId)
        {
            var bill = await _transactionRepository.GetAsync(transactionId) ?? throw new EntityNotFoundException("Bill not found");
            return _mapper.Map<BillCreationDto>(bill);
        }

        public async Task<List<BillCreationDto>> GetBillListAsync(Guid customerId)
        {
            var bills = await _transactionRepository.GetBillListAsync(customerId) ?? throw new EntityNotFoundException("No bill found");
            return _mapper.Map<List<BillCreationDto>>(bills);
        }

        public async Task<Guid> GetLastBillIdAsync(Guid customerId)
        {
            var bills = await _transactionRepository.GetBillListAsync(customerId) ?? throw new EntityNotFoundException("No bills found for the customer.");

            var lastBillId = bills.OrderByDescending(b => b.DateAndTime).FirstOrDefault()?.Id;

            return lastBillId ?? throw new EntityNotFoundException("No bills found for the customer.");
        }
    }
}
