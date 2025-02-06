/*using AutoMapper;
using MealMate.BLL.Dtos.Bills;
using MealMate.BLL.IServices;
using MealMate.BLL.IServices.Hubs;
using MealMate.BLL.IServices.Redis;
using MealMate.BLL.Services;
using MealMate.BLL.Services.Hubs;
using MealMate.DAL.Entities.Products;
using MealMate.DAL.IRepositories;
using MealMate.DAL.IRepositories.UnitOfWork;
using MealMate.DAL.Utils.Enum;
using MealMate.DAL.Utils.Exceptions;
using MealMate.DAL.Utils.GuidUtil;
using Microsoft.AspNetCore.SignalR;
using Moq;

public class TransactionServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly Mock<IAtRepository> _atRepositoryMock;
    private readonly Mock<IHubContext<ProductHub, IProductHubClient>> _productHubContextMock;
    private readonly Mock<IHubClients<IProductHubClient>> _clientsMock;
    private readonly Mock<IProductHubClient> _clientMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<GuidGenerator> _guidGeneratorMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ICustomerAppService> _customerAppServiceMock;
    private readonly Mock<IReserveCartCacheService> _reserveCartCacheServiceMock;
    private readonly Mock<ICartService> _cartServiceMock;
    private readonly TransactionService _service;

    public TransactionServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _atRepositoryMock = new Mock<IAtRepository>();
        _productHubContextMock = new Mock<IHubContext<ProductHub, IProductHubClient>>();
        _clientsMock = new Mock<IHubClients<IProductHubClient>>();
        _clientMock = new Mock<IProductHubClient>();
        _mapperMock = new Mock<IMapper>();
        _guidGeneratorMock = new Mock<GuidGenerator>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _customerAppServiceMock = new Mock<ICustomerAppService>();
        _reserveCartCacheServiceMock = new Mock<IReserveCartCacheService>();
        _cartServiceMock = new Mock<ICartService>();

        _service = new TransactionService(
            _transactionRepositoryMock.Object,
            _guidGeneratorMock.Object,
            _mapperMock.Object,
            _productRepositoryMock.Object,
            _customerAppServiceMock.Object,
            _atRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _productHubContextMock.Object,
            _reserveCartCacheServiceMock.Object,
            _cartServiceMock.Object
        );
    }

    [Fact]
    public async Task CancelOrderAsync_BillNotFound_ThrowsException()
    {
        // Arrange
        var billId = Guid.NewGuid();
        _transactionRepositoryMock.Setup(repo => repo.GetAsync(billId)).ReturnsAsync((Bill)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.CancelOrderAsync(billId, DeliveryStatus.Cancelled));
    }

    [Fact]
    public async Task CancelOrderAsync_SuccessfulCancellation_UpdatesStockAndNotifiesClients()
    {
        // Arrange
        var billId = Guid.NewGuid();
        var storeId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var bill = new Bill(billId)
        {
            PaymentMethod = "COD",
            CustomerID = Guid.NewGuid(),
            StoreID = storeId,
            DeliveryStatus = DeliveryStatus.Pending,
            Includes = new List<Include>
            {
                new Include { 
                    ProductID = productId, 
                    NumberOfProductInBill = 2,
                    Product = new Product { ProductID = productId, Price = 10 },
                    Transaction = new Bill { BillID = billId },
                    SubTotal = 20.95,
                    IsDeleted = false
                }
            }
        };
        var stock = new At { ProductID = productId, NumberAtStore = 5 };

        _transactionRepositoryMock.Setup(repo => repo.GetAsync(billId)).ReturnsAsync(bill);
        _atRepositoryMock.Setup(repo => repo.GetAtForProductsAsync(It.IsAny<List<Guid>>(), storeId))
            .ReturnsAsync(new List<At> { stock });
        _mapperMock.Setup(m => m.Map<BillDto>(It.IsAny<Bill>())).Returns(new BillDto());

        // Act
        var result = await _service.CancelOrderAsync(billId, DeliveryStatus.Cancelled);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(DeliveryStatus.Cancelled, bill.DeliveryStatus);
        Assert.Null(bill.ShipperID);
        Assert.Equal(7, stock.NumberAtStore);

        _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.RollbackTransactionAsync(), Times.Never);

        _productHubContextMock.Verify(hub => hub.Clients.Group(It.Is<string>(s => s == $"{productId}_{storeId}"))
            .ReceiveChangeStock(productId, -2), Times.Once);
    }

    [Fact]
    public async Task CancelOrderAsync_ProductNotFoundInStore_ThrowsException()
    {
        // Arrange
        var billId = Guid.NewGuid();
        var storeId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var bill = new Bill
        {
            BillID = billId,
            StoreID = storeId,
            Includes = new List<BillInclude>
            {
                new BillInclude { ProductID = productId, NumberOfProductInBill = 2 }
            }
        };

        _transactionRepositoryMock.Setup(repo => repo.GetAsync(billId)).ReturnsAsync(bill);
        _atRepositoryMock.Setup(repo => repo.GetAtForProductsAsync(It.IsAny<List<Guid>>(), storeId))
            .ReturnsAsync(new List<At>());

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.CancelOrderAsync(billId, DeliveryStatus.Cancelled));

        _unitOfWorkMock.Verify(u => u.RollbackTransactionAsync(), Times.Once);
    }
}
*/