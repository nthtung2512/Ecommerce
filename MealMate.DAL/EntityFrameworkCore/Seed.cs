using Bogus;
using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.Entities.Chatbot;
using MealMate.DAL.Entities.Products;
using MealMate.DAL.Entities.Promotion;
using MealMate.DAL.Entities.Stores;
using MealMate.DAL.Entities.Transactions;
using MealMate.DAL.Utils.Enum;
using MealMate.PL.Environment;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace MealMate.DAL.EntityFrameworkCore
{
    public class Seed
    {
        private readonly MealMateDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public Seed(MealMateDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task Populate(string strEnv)
        {
            var isValid = Enum.TryParse<AppEnvironment>(strEnv, out var env);
            if (!isValid)
                return;

            switch (env)
            {
                case AppEnvironment.Development:
                    await SeedDevelopmentStagingAsync();
                    break;
                case AppEnvironment.StagingInternal:
                    await SeedDevelopmentStagingAsync();
                    break;
            }
        }
        public static Product[] GetSeedProducts()
        {
            // Path to JSON file in the same directory as Seed.cs (at runtime)
            string jsonFilePath = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "MealMate.DAL", "EntityFrameworkCore", "cleaned_data_nometa.json")
            );


            Console.WriteLine("JSON file path: " + jsonFilePath);

            // Read the JSON file
            string jsonString = File.ReadAllText(jsonFilePath);

            // Deserialize JSON into a list of ProductDTO objects (temporary class to match JSON structure)
            var productDtos = JsonSerializer.Deserialize<List<LoadedProductDto>>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Map DTOs to Product entities with new Guids
            var products = productDtos.Select(dto => new Product(Guid.NewGuid())
            {
                Image = dto.Image ?? "",
                Consistency = dto.Consistency,
                Name = dto.Name,
                NameClean = dto.NameClean ?? "",
                OriginalName = dto.OriginalName,
                Amount = dto.Amount,
                Unit = dto.Unit,
                Price = dto.Price,
                Aisle = dto.Aisle,
                Description = dto.Description ?? "Fresh food from MealMate", // Default if null
                IsDeleted = false // Default value
            }).ToArray();

            return products;
        }

        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static Customer[] GenSeedCustomer()
        {
            var vietnameseFirstNames = new List<string>
            {
                "Cuong", "Dung", "Hung", "Khoi", "Long", "Nam", "Phat", "Quang", "Thanh", "Tung", "Tuan", "Viet", "Linh", "Hai", "Son"
            };

            var vietnameseLastNames = new List<string>
            {
                "Nguyen", "Tran", "Le", "Pham", "Huynh", "Hoang", "Phan", "Vo", "Dang", "Bui", "Do", "Ho"
            };

            // ✅ You provide these full, valid addresses
            var providedAddresses = new[]
            {
                "436 Nguyễn Thị Minh Khai, Phường 5, Quận 3, Hồ Chí Minh",
                "268 Lý Thường Kiệt, Phường 14, Quận 10, Hồ Chí Minh",
                "14 Trần Quang Diệu, Phường 14, Quận 3, Hồ Chí Minh",
                "123 Điện Biên Phủ, Phường 15, Quận Bình Thạnh, Hồ Chí Minh",
                "21 Nguyễn Văn Cừ, Phường 2, Quận 5, Hồ Chí Minh",
                "88 Cách Mạng Tháng Tám, Phường 7, Quận 3, Hồ Chí Minh",
                "19 Hai Bà Trưng, Phường Bến Nghé, Quận 1, Hồ Chí Minh",
                "300 Nguyễn Văn Linh, Phường Tân Phong, Quận 7, Hồ Chí Minh",
                "55 Phạm Văn Đồng, Phường 3, Quận Gò Vấp, Hồ Chí Minh",
                "99 Trường Chinh, Phường 13, Quận Tân Bình, Hồ Chí Minh",
            };

            var customerFaker = new Faker<Customer>("vi")
                .RuleFor(c => c.Id, f => Guid.NewGuid())
                .RuleFor(c => c.LName, f => f.PickRandom(vietnameseLastNames))
                .RuleFor(c => c.FName, f => f.PickRandom(vietnameseFirstNames))
                .RuleFor(c => c.UserName, (f, c) => $"{c.FName.ToLower().Replace(" ", "")}{f.Random.Number(1, 99)}")
                .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber("0#########"))
                .RuleFor(c => c.TotalMoneySpent, f => f.Random.Decimal(50, 500))
                .RuleFor(c => c.IsDeleted, f => false)
                .RuleFor(c => c.Email, (f, c) =>
                {
                    var localPart = $"{c.FName}.{c.LName}".ToLower().Replace(" ", "");
                    var randomDigits = f.Random.Number(100, 999); // Ensures 3 digits
                    return $"{localPart}{randomDigits}@gmail.com";
                })
                .RuleFor(c => c.FortuneChance, (f, c) => f.Random.Int(1, (int)(c.TotalMoneySpent / 100)))
                .RuleFor(c => c.Address, f => ""); // Address will be set manually

            var customers = customerFaker.Generate(providedAddresses.Length);
            var providedAddressCount = providedAddresses.Length;
            // Inject your own addresses
            for (int i = 0; i < customers.Count; i++)
            {
                customers[i].Address = providedAddresses[i % providedAddressCount];
            }

            return customers.ToArray();
        }

        public static Recipe[] GetSeedRecipes()
        {
            // Path to JSON file
            string jsonFilePath = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "MealMate.DAL", "EntityFrameworkCore", "filtered_recipes.json")
            );

            Console.WriteLine("JSON file path: " + jsonFilePath);

            // Read JSON file
            string jsonString = File.ReadAllText(jsonFilePath);

            // Deserialize JSON into a list of RecipeDTO objects
            var recipeDtos = JsonSerializer.Deserialize<List<RecipeDto>>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (recipeDtos == null)
            {
                throw new Exception("Failed to deserialize JSON file.");
            }

            // Map DTOs to Recipe entities
            var recipes = recipeDtos.Select(dto =>
            {
                var recipe = new Recipe(Guid.NewGuid())
                {
                    Title = dto.Title,
                    Tags = string.Join(";", dto.Tags),
                    Instructions = dto.Instructions ?? string.Empty,
                    Summary = dto.Summary,
                    HealthScore = dto.HealthScore,
                    ReadyInMinutes = dto.ReadyInMinutes,
                    Servings = dto.Servings,
                    Image = dto.Image ?? string.Empty
                };

                // Process Ingredients to remove duplicates (keep only last occurrence)
                var uniqueIngredients = dto.Ingredients
                    .GroupBy(ing => ing.Name, StringComparer.OrdinalIgnoreCase) // Group by Name (case-insensitive)
                    .Select(group => group.Last()) // Keep the last occurrence
                    .ToList();

                // Populate Ingredients separately since Ingredients is read-only
                recipe.Ingredients.AddRange(uniqueIngredients.Select(ing => new Ingredient
                {
                    RecipeId = recipe.Id,  // Use the Recipe's actual ID
                    Recipe = recipe,       // Set the required Recipe reference
                    Name = ing.Name,
                    Amount = ing.Amount,
                    Unit = ing.Unit,
                    Original = ing.Original,
                    NameClean = ing.NameClean ?? string.Empty
                }));

                return recipe;
            }).ToArray();

            return recipes;
        }

        public async Task SeedDevelopmentStagingAsync()
        {
            _context.Database.EnsureCreated();


            if (_context.Products.Any())
            {
                return;   // DB has been seeded
            }

            var products = GetSeedProducts();

            var customers = GenSeedCustomer();

            // Seed Shippers
            var shippers = new Shipper[]
            {
                new Shipper { Id = Guid.NewGuid(),
                    UserName = "Shipper1",
                    FName = "Sơn Tài",
                    LName = "Phan",
                    Email = "pst@gmail.com",
                    Address = "123 Phan St",
                    PhoneNumber = "1234567890",
                    IsDeleted = false,
                },
                new Shipper { Id = Guid.NewGuid(),
                    UserName = "Shipper2",
                    FName = "Bích Diễm",
                    LName = "Vũ",
                    Email = "bdv@gmail.com",
                    Address = "456 Vũ St",
                    PhoneNumber = "2345678901",
                    IsDeleted = false },
                new Shipper { Id = Guid.NewGuid(),
                    UserName = "Shipper3",
                    FName = "Tiến Vĩ",
                    LName = "Đặng",
                    Email = "hld@gmail.com",
                    Address = "789 Đặng St",
                    PhoneNumber = "3456789012",
                    IsDeleted = false},
                new Shipper { Id = Guid.NewGuid(),
                    UserName = "Shipper4",
                    FName = "Tiến Dụng",
                    LName = "Bùi",
                    Email = "hnb@gmail.com",
                    Address = "321 Bùi St",
                    PhoneNumber = "4567890123",
                    IsDeleted = false},
                new Shipper { Id = Guid.NewGuid(),
                    UserName = "Shipper5",
                    FName = "Văn Hào",
                    LName = "Đỗ",
                    Email = "ptd@gmail.com",
                    Address = "654 Đỗ St",
                    PhoneNumber = "5678901234",
                    IsDeleted = false}
            };

            // Seed Stores
            var stores = new Store[]
            {
                new Store (Guid.NewGuid()) { Name = "Store One", OpeningDate = new DateTime(2020, 1, 1).ToUniversalTime(), ContactInfo = "contact1@store.com", Location = "102 Dương Bá Trạc, Phường 2, Quận 8, Thành phố Hồ Chí Minh", Latitude = 10.7442071m, Longitude = 106.6889035m},
                new Store (Guid.NewGuid()) { Name = "Store Two", OpeningDate = new DateTime(2020, 2, 1).ToUniversalTime(), ContactInfo = "contact2@store.com", Location = "23 Pasteur, Phường Nguyễn Thái Bình, Quận 1, Hồ Chí Minh, Việt Nam", Latitude = 10.7800885m, Longitude = 106.6963455m },
                new Store (Guid.NewGuid()) { Name = "Store Three", OpeningDate = new DateTime(2020, 3, 1).ToUniversalTime(), ContactInfo = "contact3@store.com", Location = "88 Đ. Tô Hiến Thành, Phường 15, Quận 10, Hồ Chí Minh, Việt Nam", Latitude = 10.7780660m, Longitude = 106.6658002m },
                new Store (Guid.NewGuid()) { Name = "Store Four", OpeningDate = new DateTime(2020, 4, 1).ToUniversalTime(), ContactInfo = "contact4@store.com", Location = "98 Võ Văn Tần, Phường 6, Quận 3, Thành phố Hồ Chí Minh", Latitude = 10.7758044m, Longitude = 106.6893163m },
                new Store (Guid.NewGuid()) { Name = "Store Five", OpeningDate = new DateTime(2020, 5, 1).ToUniversalTime(), ContactInfo = "contact5@store.com", Location = "45 Điện Biên Phủ, Phường 15, Quận Bình Thạnh, Thành phố Hồ Chí Minh", Latitude = 10.7950647m, Longitude = 106.7012004m }
            };

            // Seed Store managers
            var storeManagers = new StoreManager[]
            {
                new StoreManager
                {
                    Id = Guid.NewGuid(),
                    UserName = "manager1",
                    FName = "Thanh Trúc",
                    LName = "Hồ",
                    Address = "123 Main St, District 1, Ho Chi Minh City",
                    Email = "manager1@store.com",
                    PhoneNumber = "0901234567",
                    IsDeleted = false,
                    Salary = 55000,
                    StoreId = stores[0].Id,
                },
                new StoreManager
                {
                    Id = Guid.NewGuid(),
                    UserName = "manager2",
                    FName = "Thùy Vân",
                    LName = "Ngô",
                    Address = "456 Park Rd, District 2, Ho Chi Minh City",
                    Email = "manager2@store.com",
                    PhoneNumber = "0902345678",
                    IsDeleted = false,
                    Salary = 57000,
                    StoreId =  stores[1].Id,
                },
                new StoreManager
                {
                    Id = Guid.NewGuid(),
                    UserName = "manager3",
                    FName = "Trường Sơn",
                    LName = "Dương",
                    Address = "789 Central Ave, District 3, Ho Chi Minh City",
                    Email = "manager3@store.com",
                    PhoneNumber = "0903456789",
                    IsDeleted = false,
                    Salary = 60000,
                    StoreId =  stores[2].Id,
                },
                new StoreManager
                {
                    Id = Guid.NewGuid(),
                    UserName = "manager4",
                    FName = "Tuấn Việt",
                    LName = "Lý",
                    Address = "321 Lakeview St, District 4, Ho Chi Minh City",
                    Email = "manager4@store.com",
                    PhoneNumber = "0904567890",
                    IsDeleted = false,
                    Salary = 62000,
                    StoreId =  stores[3].Id,
                },
                new StoreManager
                {
                    Id = Guid.NewGuid(),
                    UserName = "manager5",
                    FName = "Tuấn Khôi",
                    LName = "Phùng",
                    Address = "654 Hilltop St, District 5, Ho Chi Minh City",
                    Email = "manager5@store.com",
                    PhoneNumber = "0905678901",
                    IsDeleted = false,
                    Salary = 65000,
                    StoreId = stores[4].Id,
                }
            };

            // Seed Admin
            var admin = new ApplicationUser
            {
                Id = new Guid("7a7c64d4-dd66-4174-af76-4f21aaa8c70b"),
                UserName = "Admin",
                FName = "Anh Bảo",
                LName = "Nguyễn",
                Address = "Admin Address",
                Email = "nguyenanhbao@gmail.com",
                PhoneNumber = "1234567890",
                IsDeleted = false
            };

            /*            var test = new string[]
                            {
                                "customer1", "customer2", "customer3", "customer4"
                            };

                        for (int i = 0; i < customers.Length; i++)
                        {
                            customers[i].SecurityStamp = Guid.NewGuid().ToString();
                            var result1 = await _userManager.CreateAsync(customers[i], test[i]);
                            if (result1.Succeeded)
                            {
                                // Optionally add the user to a role
                                await _userManager.AddToRoleAsync(customers[i], "Customer");
                            }
                        }*/

            // Seed ApplicationUser
            if (!_context.ApplicationUsers.Any())
            {
                var customerPasswords = Enumerable.Range(1, 20)
                .Select(i => $"customer{i}")
                .ToArray();
                var shipperPasswords = new string[]
                {
                    "shipper1", "shipper2", "shipper3", "shipper4", "shipper5"
                };
                var managerPasswords = new string[]
                {
                    "manager1", "manager2", "manager3", "manager4", "manager5"
                };
                var adminPassword = "admin";

                for (int i = 0; i < customers.Length; i++)
                {
                    customers[i].SecurityStamp = Guid.NewGuid().ToString();
                    var result1 = await _userManager.CreateAsync(customers[i], customerPasswords[i]);
                    if (result1.Succeeded)
                    {
                        // Optionally add the user to a role
                        await _userManager.AddToRoleAsync(customers[i], "Customer");
                    }
                }

                for (int i = 0; i < 5; i++)
                {
                    shippers[i].SecurityStamp = Guid.NewGuid().ToString();
                    var result2 = await _userManager.CreateAsync(shippers[i], shipperPasswords[i]);
                    if (result2.Succeeded)
                    {
                        // Optionally add the user to a role
                        await _userManager.AddToRoleAsync(shippers[i], "Shipper");
                    }
                }

                for (int i = 0; i < 5; i++)
                {
                    storeManagers[i].SecurityStamp = Guid.NewGuid().ToString();
                    var result3 = await _userManager.CreateAsync(storeManagers[i], managerPasswords[i]);
                    if (result3.Succeeded)
                    {
                        // Optionally add the user to a role
                        await _userManager.AddToRoleAsync(storeManagers[i], "StoreManager");
                    }
                }

                admin.SecurityStamp = Guid.NewGuid().ToString();
                var result = await _userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    // Optionally add the user to a role
                    await _userManager.AddToRoleAsync(admin, "Admin");
                }
            };



            var atEntities = new List<AT>();

            // Store 1 gets **every product**
            var store1 = stores.First(); // assuming store 1 is the first one

            foreach (var product in products)
            {
                var atEntity = new AT
                {
                    ProductID = product.Id,
                    Product = product,
                    StoreID = store1.Id,
                    Store = store1,
                    NumberAtStore = new Random().Next(10, 50),
                    IsDeleted = false
                };

                atEntities.Add(atEntity);
            }

            // For remaining stores, each gets 70% of the products
            var remainingStores = stores.Skip(1).ToList();
            var productCount = products.Count();
            var seventyPercentCount = (int)(productCount * 0.7);

            foreach (var store in remainingStores)
            {
                // Shuffle and take 70% of products for this store
                var selectedProducts = products.OrderBy(_ => Guid.NewGuid()).Take(seventyPercentCount).ToList();

                foreach (var product in selectedProducts)
                {
                    var atEntity = new AT
                    {
                        ProductID = product.Id,
                        Product = product,
                        StoreID = store.Id,
                        Store = store,
                        NumberAtStore = new Random().Next(5, 30),
                        IsDeleted = false
                    };

                    atEntities.Add(atEntity);
                }
            }

            // Seed Bills
            // Generate Bill and Include data
            var random = new Random();
            var bills = new List<Bill>();

            foreach (var customer in customers)
            {
                int numberOfBills = random.Next(3, 7);

                for (int i = 0; i < numberOfBills; i++)
                {
                    var store = stores[random.Next(stores.Count())];
                    var bill = new Bill(Guid.NewGuid())
                    {
                        PaymentMethod = random.Next(0, 2) == 0 ? "Credit Card" : "Momo",
                        DateAndTime = DateTime.UtcNow.AddDays(-random.Next(1, 30)),
                        CustomerID = customer.Id,
                        Customer = customer,
                        StoreID = store.Id,
                        Store = store,
                        TotalPrice = 0,
                        DeliveryStatus = (DeliveryStatus)random.Next(1, Enum.GetValues(typeof(DeliveryStatus)).Length),
                        ShippingAddress = customer.Address,
                        IsDeleted = false
                    };

                    // Make 1 out of 6 bills pending without a shipper
                    if (i == numberOfBills - 1)
                    {
                        bill.DeliveryStatus = DeliveryStatus.Pending;
                        bill.ShipperID = null;
                        bill.Shipper = null;
                    }
                    else
                    {
                        var shipper = shippers[random.Next(shippers.Count())];
                        bill.ShipperID = shipper.Id;
                        bill.Shipper = shipper;
                    }

                    // Generate Includes
                    int numberOfItems = random.Next(4, 10);
                    double billTotalPrice = 0;
                    var firstIndex = random.Next(products.Length);

                    for (int j = 0; j < numberOfItems; j++)
                    {
                        var product = products[(firstIndex + j) % products.Length];
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
            // Now `bills` contains the generated Bill and Include data

            // Seed Promotions
            var productPromotions = new ProductPromotion[]
            {
               new ProductPromotion(Guid.NewGuid())
               {
                    Discount = 0.15m,
                    Name = "Summer Sale",
                    Description = "Enjoy summer feast with 15% off",
                    StartDay = new DateTime(2024, 11, 01).ToUniversalTime(),
                    EndDay = new DateTime(2025, 12, 30).ToUniversalTime()
               },
                new ProductPromotion(Guid.NewGuid())
                {
                    Discount = 0.10m,
                    Name = "Winter Clearance",
                    Description = "Hot stew in winter with 10% off",
                    StartDay = new DateTime(2024, 11, 01).ToUniversalTime(),
                    EndDay = new DateTime(2025, 12, 30).ToUniversalTime()
                },
                new ProductPromotion(Guid.NewGuid())
                {
                    Discount = 0.20m,
                    Name = "Holiday Special",
                    Description = "Have fun with your family with 20% off",
                    StartDay = new DateTime(2025, 01, 01).ToUniversalTime(),
                    EndDay = new DateTime(2025, 02, 20).ToUniversalTime()
                },
                new ProductPromotion(Guid.NewGuid())
                {
                    Discount = 0.20m,
                    Name = "Holiday Special expir",
                    Description = "Expr Have fun with your family with 20% off",
                    StartDay = new DateTime(2024, 01, 01).ToUniversalTime(),
                    EndDay = new DateTime(2024, 01, 30).ToUniversalTime()
                }
            };

            // Assume you want to apply promotions to random products
            var random1 = new Random();
            var promoteProducts = new List<PromoteProduct>();

            foreach (var promotion in productPromotions)
            {
                // Calculate 15% of products (rounded down)
                int count = (int)(products.Length * 0.15);

                // Select random 15% of products
                var selectedProducts = products.OrderBy(p => Guid.NewGuid()).Take(count).ToList();

                foreach (var product in selectedProducts)
                {
                    var promoteProduct = new PromoteProduct
                    {
                        ProductId = product.Id,
                        Product = product,
                        PromotionId = promotion.Id,
                        ProductPromotion = promotion
                    };

                    promoteProducts.Add(promoteProduct);
                    promotion.PromoteProducts.Add(promoteProduct);
                }
            }

            var billPromotions = new BillPromotion[]
            {
                new BillPromotion(Guid.NewGuid())
                {
                    Discount = 0.10m,
                    Name = "Cashback Offer",
                    Description = "10% cashback on all purchases over $100",
                    StartDay = new DateTime(2024, 11, 01).ToUniversalTime(),
                    EndDay = new DateTime(2025, 12, 30).ToUniversalTime(),
                    ApplyPrice = 100,
                    PromotionChance = 50
                },
                new BillPromotion(Guid.NewGuid())
                {
                    Discount = 0.05m,
                    Name = "Weekend Special",
                    Description = "5% off for bills over $50",
                    StartDay = new DateTime(2024, 11, 01).ToUniversalTime(),
                    EndDay = new DateTime(2025, 12, 30).ToUniversalTime(),
                    ApplyPrice = 50,
                    PromotionChance = 30
                },
                new BillPromotion(Guid.NewGuid())
                {
                    Discount = 0.15m,
                    Name = "VIP Discount",
                    Description = "15% off for bills over $200",
                    StartDay = new DateTime(2024, 11, 01).ToUniversalTime(),
                    EndDay = new DateTime(2024, 12, 30).ToUniversalTime(),
                    ApplyPrice = 200,
                    PromotionChance = 20
                },
                new BillPromotion(Guid.NewGuid())
                {
                    Discount = 0.10m,
                    Name = "VIP Discount",
                    Description = "10% off for bills over $300",
                    StartDay = new DateTime(2025, 01, 01).ToUniversalTime(),
                    EndDay = new DateTime(2025, 02, 20).ToUniversalTime(),
                    ApplyPrice = 200,
                    PromotionChance = 20
                }
            };

            // Logic to apply BillPromotions to Bills
            var promoteBills = new List<PromoteBill>();

            // Iterate through each bill
            foreach (var bill in bills)
            {
                // Find all eligible promotions where bill's total price >= promotion's ApplyPrice
                var promotions = billPromotions
                    .Where(p => bill.TotalPrice >= p.ApplyPrice && p.StartDay <= DateTime.UtcNow.AddHours(7) && p.EndDay >= DateTime.UtcNow.AddHours(7))
                    .OrderByDescending(p => p.ApplyPrice) // Sort by ApplyPrice in descending order to get the highest one
                    .ToList();

                if (promotions.Count == 0) continue; // No promotion available for this bill

                var bestPromotion = promotions[0]; // Get the best promotion

                // Create a new PromoteBill object and associate it with the Bill and the best BillPromotion
                var promoteBill = new PromoteBill
                {
                    TransactionId = bill.Id,
                    Bill = bill,
                    PromotionId = bestPromotion.Id,
                    BillPromotion = bestPromotion
                };

                promoteBills.Add(promoteBill); // Add the PromoteBill to the list of PromoteBills

                bestPromotion.PromoteBills.Add(promoteBill); // Add the PromoteBill to the BillPromotion's PromoteBills collection

                // Add the PromoteBill to the Bill's PromoteBills collection
                bill.PromoteBill = promoteBill;
            }

            var customerPromotions = new CustomerPromotion[]
            {
                new CustomerPromotion(Guid.NewGuid())
                {
                    Discount = 0.25m,
                    Name = "New Ground Beef try out",
                    Description = "20% off for Ground Beef",
                    StartDay = new DateTime(2024, 11, 01).ToUniversalTime(),
                    EndDay = new DateTime(2025, 11, 30).ToUniversalTime(),
                    ProductId = products[18].Id
                },
                new CustomerPromotion(Guid.NewGuid())
                {
                    Discount = 0.15m,
                    Name = "New Rice discount",
                    Description = "15% off on Rice",
                    StartDay = new DateTime(2024, 11, 01).ToUniversalTime(),
                    EndDay = new DateTime(2025, 11, 30).ToUniversalTime(),
                    ProductId = products[104].Id
                },
                new CustomerPromotion(Guid.NewGuid())
                {
                    Discount = 0.20m,
                    Name = "Fish Fillet discount",
                    Description = "20% off on Fish Fillet",
                    StartDay = new DateTime(2024, 11, 01).ToUniversalTime(),
                    EndDay = new DateTime(2025, 11, 30).ToUniversalTime(),
                    ProductId = products[1084].Id
                },
                new CustomerPromotion(Guid.NewGuid())
                {
                    Discount = 0.15m,
                    Name = "New Bananas try out",
                    Description = "15% off for Beef Ribeye",
                    StartDay = new DateTime(2024, 11, 01).ToUniversalTime(),
                    EndDay = new DateTime(2025, 11, 30).ToUniversalTime(),
                    ProductId = products[541].Id
                },
                new CustomerPromotion(Guid.NewGuid())
                {
                    Discount = 0.2m,
                    Name = "Whipped Cream discount",
                    Description = "20% off on Whipped Cream",
                    StartDay = new DateTime(2024, 11, 01).ToUniversalTime(),
                    EndDay = new DateTime(2025, 11, 30).ToUniversalTime(),
                    ProductId = products[490].Id
                }
            };

            var promoteCustomers = new List<PromoteCustomer>();
            var random3 = new Random();

            // Get counts based on percentages
            int totalCustomers = customers.Length;
            int count0 = (int)(totalCustomers * 0.6);
            int count1 = (int)(totalCustomers * 0.2);
            int count2 = (int)(totalCustomers * 0.1);
            int count3 = totalCustomers - (count0 + count1 + count2); // Whatever remains

            // Create a list matching the promotion counts per customer
            var promotionDistribution = new List<int>();
            promotionDistribution.AddRange(Enumerable.Repeat(0, count0));
            promotionDistribution.AddRange(Enumerable.Repeat(1, count1));
            promotionDistribution.AddRange(Enumerable.Repeat(2, count2));
            promotionDistribution.AddRange(Enumerable.Repeat(3, count3));

            // Shuffle the distribution
            promotionDistribution = promotionDistribution.OrderBy(_ => Guid.NewGuid()).ToList();

            // Assign promotions to customers
            for (int i = 0; i < totalCustomers; i++)
            {
                var customer = customers[i];
                int promotionCount = promotionDistribution[i];

                var selectedPromotions = customerPromotions
                    .OrderBy(_ => Guid.NewGuid())
                    .Take(promotionCount);

                foreach (var promotion in selectedPromotions)
                {
                    var promoteCustomer = new PromoteCustomer
                    {
                        CustomerId = customer.Id,
                        Customer = customer,
                        PromotionId = promotion.Id,
                        CustomerPromotion = promotion
                    };

                    promoteCustomers.Add(promoteCustomer);
                    promotion.PromoteCustomers.Add(promoteCustomer);
                }
            }

            // Step 1: Generate and collect ratings
            var recipes = GetSeedRecipes();
            var randomVal = new Random();
            var customerToSkip = customers[randomVal.Next(customers.Length)];
            var ratings = new List<RecipeRating>();

            foreach (var customer in customers)
            {
                if (customer.Id == customerToSkip.Id)
                    continue;

                int recipeCountToRate = randomVal.Next((int)(recipes.Length * 0.05), (int)(recipes.Length * 0.15) + 1);
                var recipesToRate = recipes.OrderBy(_ => randomVal.Next()).Take(recipeCountToRate).ToList();

                foreach (var recipe in recipesToRate)
                {
                    decimal[] possibleRatings = { 3.0m, 3.5m, 4.0m, 4.5m, 5.0m };
                    decimal ratingValue = possibleRatings[randomVal.Next(possibleRatings.Length)];

                    ratings.Add(new RecipeRating
                    {
                        CustomerId = customer.Id,
                        RecipeId = recipe.Id,
                        Rating = ratingValue,
                        Timestamp = DateTime.UtcNow.AddDays(-random.Next(1, 30))
                    });
                }
            }

            // Step 2: Update AverageRating in-memory
            var recipeGroups = ratings
                .GroupBy(r => r.RecipeId)
                .ToDictionary(g => g.Key, g => g.Select(r => r.Rating).ToList());

            foreach (var recipe in recipes)
            {
                if (recipeGroups.TryGetValue(recipe.Id, out var ratingList))
                {
                    recipe.AverageRating = Math.Round(ratingList.Average(), 1);
                }
            }

            // Continue adding similar blocks for other entities like BillPromotion, CustomerPromotion, Shipper, StoreManager, Store, etc.
            // Add to the database context
            /*_context.ApplicationUsers.AddRange(allUsers);*/
            /*            _context.Customers.AddRange(customers);
                        _context.Shippers.AddRange(shippers);
                        _context.StoreManagers.AddRange(storeManagers);*/

            _context.Stores.AddRange(stores);
            _context.Products.AddRange(products);
            if (!_context.ATs.Any())
                _context.ATs.AddRange(atEntities);
            _context.Bills.AddRange(bills);
            if (!_context.Includes.Any())
                _context.Includes.AddRange(bills.SelectMany(b => b.Includes));
            _context.ProductPromotions.AddRange(productPromotions);
            if (!_context.PromoteProducts.Any())
                _context.PromoteProducts.AddRange(promoteProducts);
            _context.BillPromotions.AddRange(billPromotions);
            if (!_context.PromoteBills.Any())
                _context.PromoteBills.AddRange(promoteBills);
            _context.CustomerPromotions.AddRange(customerPromotions);
            if (!_context.PromoteCustomers.Any())
                _context.PromoteCustomers.AddRange(promoteCustomers);
            if (!_context.Recipes.Any())
                _context.Recipes.AddRange(recipes);
            if (!_context.RecipeRatings.Any())
                _context.RecipeRatings.AddRange(ratings);

            await _context.SaveChangesAsync();
        }
    }
}

public class LoadedProductDto
{
    public int Id { get; set; } // Ignored in Product class
    public string Image { get; set; }
    public string Consistency { get; set; }
    public string Name { get; set; }
    public string NameClean { get; set; }
    public string OriginalName { get; set; }
    public int Amount { get; set; }
    public string Unit { get; set; }
    public double Price { get; set; }
    public string Aisle { get; set; }
    public string Description { get; set; } // Optional in JSON, will use default if missing
}

// DTO classes for JSON mapping
public class RecipeDto
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required List<string> Tags { get; set; }
    public required List<IngredientDto> Ingredients { get; set; }
    public required string Instructions { get; set; }
    public required string Summary { get; set; }
    public required int HealthScore { get; set; }
    public required int ReadyInMinutes { get; set; }
    public required int Servings { get; set; }
    public required string Image { get; set; }
}

public class IngredientDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required double Amount { get; set; }
    public required string Unit { get; set; }
    public required string Original { get; set; }
    public required string NameClean { get; set; }
}
