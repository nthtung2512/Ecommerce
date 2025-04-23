using MealMate.BLL.IServices.Utility;
using MealMate.DAL.Entities.Products;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils.Enum;

namespace MealMate.BLL.Services.Utility
{
    internal class HandleCreateBill : IHandleCreateBill
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IShipperRepository _shipperRepository;
        private readonly IProductRepository _productRepository;
        private readonly ITransactionRepository _transactionRepository;

        public HandleCreateBill(ICustomerRepository customerRepository, IStoreRepository storeRepository, IShipperRepository shipperRepository, IProductRepository productRepository, ITransactionRepository transactionRepository)
        {
            _customerRepository = customerRepository;
            _storeRepository = storeRepository;
            _shipperRepository = shipperRepository;
            _productRepository = productRepository;
            _transactionRepository = transactionRepository;
        }

        private DeliveryStatus GetWeightedRandomStatus(Random random)
        {
            int roll = random.Next(1, 101); // 1 to 100

            if (roll <= 70)
                return DeliveryStatus.Delivered;     // 70%
            else if (roll <= 90)
                return DeliveryStatus.Cancelled;     // next 20%
            else
                return DeliveryStatus.Ghost;         // last 10%
        }

        public async Task CreateBulkBillPrevDay(Guid storeId)
        {
            // Seed Bills
            // Generate Bill and Include data
            var random = new Random();
            var bills = new List<Bill>();

            var customers = await _customerRepository.GetCustomersListAsync();
            var stores = await _storeRepository.GetAllStoresAsync();
            var shippers = await _shipperRepository.GetListAsync();
            var productsAtStore = await _productRepository.GetListProductByStoreIDAsync(storeId);

            foreach (var customer in customers)
            {
                int numberOfBills = 20;

                for (int i = 0; i < numberOfBills; i++)
                {
                    var store = stores[random.Next(stores.Count())];
                    var bill = new Bill(Guid.NewGuid())
                    {
                        PaymentMethod = random.Next(0, 2) == 0 ? "Credit Card" : "Momo",
                        DateAndTime = DateTime.UtcNow.AddDays(-1),
                        CustomerID = customer.Id,
                        Customer = customer,
                        StoreID = store.Id,
                        Store = store,
                        TotalPrice = 0,
                        DeliveryStatus = GetWeightedRandomStatus(random),
                        ShippingAddress = customer.Address,
                        IsDeleted = false
                    };

                    var shipper = shippers[random.Next(shippers.Count())];
                    bill.ShipperID = shipper.Id;
                    bill.Shipper = shipper;

                    // Generate Includes
                    int numberOfItems = random.Next(3, 6);
                    double billTotalPrice = 0;
                    var firstIndex = random.Next(productsAtStore.Count);

                    for (int j = 0; j < numberOfItems; j++)
                    {
                        var product = productsAtStore[(firstIndex + j) % productsAtStore.Count];
                        int numberOfProducts = random.Next(1, 5);
                        double subTotal = product.Price * numberOfProducts;

                        var include = new Include
                        {
                            TransactionID = bill.Id,
                            ProductID = product.Id,
                            Transaction = bill,
                            Product = product,
                            NumberOfProductInBill = numberOfProducts,
                            SubTotal = subTotal,
                            IsDeleted = false
                        };

                        bill.Includes.Add(include);
                        billTotalPrice += subTotal;
                    }

                    bill.TotalPrice = billTotalPrice;
                    bills.Add(bill);
                }
            }
            foreach (var bill in bills)
            {
                await _transactionRepository.CreateAsync(bill);
            }
        }
    }
}
