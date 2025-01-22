using AutoMapper;
using MealMate.BLL.Dtos.Bills;
using MealMate.BLL.Dtos.Product;
using MealMate.BLL.Dtos.Stores;
using MealMate.BLL.IServices;
using MealMate.BLL.IServices.Hubs;
using MealMate.BLL.IServices.Redis;
using MealMate.BLL.Services.Hubs;
using MealMate.DAL.Entities.Products;
using MealMate.DAL.IRepositories;
using MealMate.DAL.IRepositories.CartRedis;
using MealMate.DAL.IRepositories.UnitOfWork;
using MealMate.DAL.Utils.Enum;
using MealMate.DAL.Utils.Exceptions;
using MealMate.DAL.Utils.GuidUtil;
using Microsoft.AspNetCore.SignalR;

namespace MealMate.BLL.Services
{
    internal class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAtRepository _atRepository;
        private readonly ICustomerAppService _customerAppService;
        private readonly IReserveCartCacheService _reserveCartCacheService;
        private readonly ICartService _cartService;
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ProductHub, IProductHubClient> _productHubContext;
        private readonly GuidGenerator _guidGenerator;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, GuidGenerator guidGenerator, IMapper mapper, IProductRepository productRepository, ICustomerAppService customerAppService, IAtRepository atRepository, IUnitOfWork unitOfWork, IHubContext<ProductHub, IProductHubClient> productHubContext, IReserveCartCacheService reserveCartCacheService, ICartService cartService, ICartRepository cartRepository)
        {
            _transactionRepository = transactionRepository;
            _guidGenerator = guidGenerator;
            _mapper = mapper;
            _productRepository = productRepository;
            _customerAppService = customerAppService;
            _atRepository = atRepository;
            _unitOfWork = unitOfWork;
            _productHubContext = productHubContext;
            _reserveCartCacheService = reserveCartCacheService;
            _cartService = cartService;
            _cartRepository = cartRepository;
        }

        public async Task<List<BillDto>> GetAllBillAsync()
        {
            var bills = await _transactionRepository.GetAllBillAsync();
            if (bills.Count == 0)
            {
                throw new EntityNotFoundException("No bills found");
            }
            return _mapper.Map<List<BillDto>>(bills);
        }
        public async Task<List<BillDto>> GetBillListAsync(Guid customerId)
        {
            var bills = await _transactionRepository.GetBillListAsync(customerId) ?? throw new EntityNotFoundException("No bill found");
            return _mapper.Map<List<BillDto>>(bills);
        }

        public async Task<FullBillDto> GetBillByIdAsync(Guid transactionId)
        {
            var bill = await _transactionRepository.GetAsync(transactionId) ?? throw new EntityNotFoundException("Bill not found");
            var includesDto = bill.Includes.Select(include => new IncludeDto
            {
                TransactionID = include.TransactionID,
                ProductID = include.ProductID,
                NumberOfProductInBill = include.NumberOfProductInBill,
                SubTotal = include.SubTotal,
                Product = _mapper.Map<ProductCreationDto>(include.Product)
            }).ToList();

            var fullBillDto = new FullBillDto
            {
                TransactionId = bill.Id,
                CustomerID = bill.CustomerID,
                StoreID = bill.StoreID,
                ShipperID = bill.ShipperID,
                PaymentMethod = bill.PaymentMethod,
                DateAndTime = bill.DateAndTime,
                DeliveryStatus = bill.DeliveryStatus,
                TotalPrice = bill.TotalPrice,
                TotalWeight = bill.TotalWeight,
                Includes = includesDto
            };
            return fullBillDto;
        }

        public async Task<Guid> GetLastBillIdAsync(Guid customerId)
        {
            var bills = await _transactionRepository.GetBillListAsync(customerId) ?? throw new EntityNotFoundException("No bills found for the customer.");

            var lastBillId = bills.OrderByDescending(b => b.DateAndTime).FirstOrDefault()?.Id;

            return lastBillId ?? throw new EntityNotFoundException("No bills found for the customer.");
        }

        public async Task<FullBillDto> CreateBillAsync(BillCreationDto billData)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var newId = _guidGenerator.Create();
                var newBill = new Bill(newId)
                {
                    PaymentMethod = billData.PaymentMethod,
                    DateAndTime = billData.DateAndTime,
                    CustomerID = billData.CustomerID,
                    StoreID = billData.StoreID,
                    ShipperID = null,
                    TotalPrice = billData.TotalPrice,
                    TotalWeight = billData.TotalWeight,
                    DeliveryStatus = DeliveryStatus.Pending,
                    ShippingAddress = billData.ShippingAddress,
                    IsDeleted = false
                };

                var includes = new List<Include>();
                foreach (var includeDto in billData.Includes)
                {
                    var product = await _productRepository.GetAsync(includeDto.ProductID)
                        ?? throw new EntityNotFoundException("Product not found");

                    var at = await _atRepository.GetAtByProductIDAndStoreIDAsync(includeDto.ProductID, newBill.StoreID)
                        ?? throw new EntityNotFoundException("No product found at store.");

                    at.NumberAtStore -= includeDto.NumberOfProductInBill;
                    await _atRepository.UpdateAsync(at);

                    includes.Add(new Include
                    {
                        TransactionID = newId,
                        ProductID = includeDto.ProductID,
                        Product = product,
                        Transaction = newBill,
                        NumberOfProductInBill = includeDto.NumberOfProductInBill,
                        SubTotal = includeDto.SubTotal,
                        IsDeleted = false
                    });
                }

                newBill.Includes.AddRange(includes);
                await _transactionRepository.CreateAsync(newBill);

                var includesDto = newBill.Includes.Select(include => new IncludeDto
                {
                    TransactionID = include.TransactionID,
                    ProductID = include.ProductID,
                    NumberOfProductInBill = include.NumberOfProductInBill,
                    SubTotal = include.SubTotal,
                    Product = _mapper.Map<ProductCreationDto>(include.Product)
                }).ToList();

                await _reserveCartCacheService.RemoveReserveCartAsync(billData.CustomerID);
                await _cartService.ClearCartAsync(billData.CustomerID);

                await _unitOfWork.CommitTransactionAsync();

                foreach (var include in includes)
                {
                    await _productHubContext.Clients.Group($"{include.ProductID}_{billData.StoreID}").ReceiveChangeStock(include.ProductID, include.NumberOfProductInBill);
                }

                return new FullBillDto
                {
                    TransactionId = newBill.Id,
                    CustomerID = newBill.CustomerID,
                    StoreID = newBill.StoreID,
                    ShipperID = newBill.ShipperID,
                    PaymentMethod = newBill.PaymentMethod,
                    DateAndTime = newBill.DateAndTime,
                    DeliveryStatus = newBill.DeliveryStatus,
                    TotalPrice = newBill.TotalPrice,
                    TotalWeight = newBill.TotalWeight,
                    Includes = includesDto
                };
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }


        public async Task<BillDto> AssignShipperToBillAsync(Guid transactionId, Guid shipperId)
        {
            var bill = await _transactionRepository.GetAsync(transactionId) ?? throw new EntityNotFoundException("Bill not found");
            bill.ShipperID = shipperId;
            await _transactionRepository.UpdateAsync(bill);
            return _mapper.Map<BillDto>(bill);
        }

        public async Task<DeliveryStatus> UpdateDeliveryStatusAsync(Guid transactionId, DeliveryStatus status)
        {
            var bill = await _transactionRepository.GetAsync(transactionId) ?? throw new EntityNotFoundException("Bill not found");
            bill.DeliveryStatus = status;
            if (status == DeliveryStatus.Delivered)
            {
                await _customerAppService.AddTotalMoneySpentByIdAsync(bill.CustomerID, (decimal)Math.Round(bill.TotalPrice, 2));
            }
            await _transactionRepository.UpdateAsync(bill);
            return status;
        }

        public async Task<List<FullBillDto>> GetBillListByStoreIdAsync(Guid storeId, DeliveryStatus status)
        {
            var bills = await _transactionRepository.GetBillListByStoreIdAsync(storeId, status);
            if (bills.Count == 0)
            {
                throw new EntityNotFoundException("No bills found");
            }

            var fullBillDtos = new List<FullBillDto>();
            foreach (var bill in bills)
            {
                var includesDto = bill.Includes.Select(include => new IncludeDto
                {
                    TransactionID = include.TransactionID,
                    ProductID = include.ProductID,
                    NumberOfProductInBill = include.NumberOfProductInBill,
                    SubTotal = include.SubTotal,
                    Product = _mapper.Map<ProductCreationDto>(include.Product)
                }).ToList();

                var fullBillDto = new FullBillDto
                {
                    TransactionId = bill.Id,
                    CustomerID = bill.CustomerID,
                    StoreID = bill.StoreID,
                    ShipperID = bill.ShipperID,
                    PaymentMethod = bill.PaymentMethod,
                    DateAndTime = bill.DateAndTime,
                    DeliveryStatus = bill.DeliveryStatus,
                    TotalPrice = bill.TotalPrice,
                    TotalWeight = bill.TotalWeight,
                    Includes = includesDto
                };
                fullBillDtos.Add(fullBillDto);
            }

            return fullBillDtos;
        }
    }
}
