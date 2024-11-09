using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.Entities.Products;
using MealMate.DAL.Entities.Promotion;
using MealMate.DAL.Entities.Stores;
using MealMate.DAL.Entities.Transactions;
using MealMate.DAL.Utils.Enum;
using MealMate.PL.Environment;
using Microsoft.AspNetCore.Identity;

namespace MealMate.DAL.EntityFrameworkCore
{
    public class Seed
    {
        private readonly MealMateDbContext _context;
        private readonly PasswordHasher<ApplicationUser> _passwordHasher;
        private readonly UserManager<ApplicationUser> _userManager;
        public Seed(MealMateDbContext context, PasswordHasher<ApplicationUser> passwordHasher, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _passwordHasher = passwordHasher;
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
        public async Task SeedDevelopmentStagingAsync()
        {
            _context.Database.EnsureCreated();

            if (_context.Products.Any())
            {
                return;   // DB has been seeded
            }

            /*var allUsers = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = new Guid("3fd99782-4556-4bbc-bf79-8e5ade728fc4"),
                    UserName = "Customer1",
                    FName = "Cường Dũng",
                    LName = "Trần",
                    Address = "123 Elm St",
                    Email = "nguyenthanhtung@gmail.com",
                    PhoneNumber = "0987654321",
                    IsDeleted = false
                },
                new ApplicationUser
                {
                    Id = new Guid("fcdedd40-4b84-45ad-9d30-783264f83f61"),
                    UserName = "Customer2",
                    FName = "Hưng Khôi",
                    LName = "Lê",
                    Address = "456 Oak St",
                    Email = "lhk@gmail.com",
                    PhoneNumber = "1234567812",
                    IsDeleted = false
                },
                new ApplicationUser
                {
                    Id = new Guid("42f52f54-a89a-49cf-8564-4c116987e237"),
                    UserName = "Customer3",
                    FName = "Long Nam",
                    LName = "Phạm",
                    Address = "789 Pine St",
                    Email = "pln@gmail.com",
                    PhoneNumber = "1234876521",
                    IsDeleted = false
                },
                new ApplicationUser
                {
                    Id = new Guid("8da727e0-9529-49cf-a971-945791782208"),
                    UserName = "Customer4",
                    FName = "Phát Quang",
                    LName = "Huỳnh",
                    Address = "321 Maple St",
                    Email = "hpq@gmail.com",
                    PhoneNumber = "3218764521",
                    IsDeleted = false
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "Shipper1",
                    FName = "Sơn Tài",
                    LName = "Phan",
                    Email = "pst@gmail.com",
                    Address = "123 Phan St",
                    PhoneNumber = "1234567890",
                    IsDeleted = false
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "Shipper2",
                    FName = "Bích Diễm",
                    LName = "Vũ",
                    Email = "bdv@gmail.com",
                    Address = "456 Vũ St",
                    PhoneNumber = "2345678901",
                    IsDeleted = false
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "Shipper3",
                    FName = "Tiến Vĩ",
                    LName = "Đặng",
                    Email = "hld@gmail.com",
                    Address = "789 Đặng St",
                    PhoneNumber = "3456789012",
                    IsDeleted = false
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "Shipper4",
                    FName = "Tiến Dụng",
                    LName = "Bùi",
                    Email = "hnb@gmail.com",
                    Address = "321 Bùi St",
                    PhoneNumber = "4567890123",
                    IsDeleted = false
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "Shipper5",
                    FName = "Văn Hào",
                    LName = "Đỗ",
                    Email = "ptd@gmail.com",
                    Address = "654 Đỗ St",
                    PhoneNumber = "5678901234",
                    IsDeleted = false
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "manager1",
                    FName = "Thanh Trúc",
                    LName = "Hồ",
                    Address = "123 Main St, District 1, Ho Chi Minh City",
                    Email = "manager1@store.com",
                    PhoneNumber = "0901234567",
                    IsDeleted = false
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "manager2",
                    FName = "Thùy Vân",
                    LName = "Ngô",
                    Address = "456 Park Rd, District 2, Ho Chi Minh City",
                    Email = "manager2@store.com",
                    PhoneNumber = "0902345678",
                    IsDeleted = false
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "manager3",
                    FName = "Trường Sơn",
                    LName = "Dương",
                    Address = "789 Central Ave, District 3, Ho Chi Minh City",
                    Email = "manager3@store.com",
                    PhoneNumber = "0903456789",
                    IsDeleted = false
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "manager4",
                    FName = "Tuấn Việt",
                    LName = "Lý",
                    Address = "321 Lakeview St, District 4, Ho Chi Minh City",
                    Email = "manager4@store.com",
                    PhoneNumber = "0904567890",
                    IsDeleted = false
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "manager5",
                    FName = "Tuấn Khôi",
                    LName = "Phùng",
                    Address = "654 Hilltop St, District 5, Ho Chi Minh City",
                    Email = "manager5@store.com",
                    PhoneNumber = "0905678901",
                    IsDeleted = false
                },
                new ApplicationUser
                {
                    Id = new Guid("7a7c64d4-dd66-4174-af76-4f21aaa8c70b"),
                    UserName = "Admin",
                    FName = "Anh Bảo",
                    LName = "Nguyễn",
                    Address = "Admin Address",
                    Email = "nguyenanhbao@gmail.com",
                    PhoneNumber = "1234567890",
                    IsDeleted = false
                }
            };
*/

            var customers = new Customer[] {
                new Customer
                {
                    Id = new Guid("3fd99782-4556-4bbc-bf79-8e5ade728fc4"),
                    UserName = "Customer1",
                    FName = "Cường Dũng",
                    LName = "Trần",
                    Address = "123 Elm St",
                    Email = "nguyenthanhtung@gmail.com",
                    PhoneNumber = "0987654321",
                    IsDeleted = false,
                    TotalMoneySpent = 150.00M,
                    FortuneChance = 5,
                },
                new Customer
                {
                    Id = new Guid("fcdedd40-4b84-45ad-9d30-783264f83f61"),
                    UserName = "Customer2",
                    FName = "Hưng Khôi",
                    LName = "Lê",
                    Address = "456 Oak St",
                    Email = "lhk@gmail.com",
                    PhoneNumber = "1234567812",
                    IsDeleted = false,
                    TotalMoneySpent = 200.00M,
                    FortuneChance = 3,
                },
                new Customer
                {
                    Id = new Guid("42f52f54-a89a-49cf-8564-4c116987e237"),
                    UserName = "Customer3",
                    FName = "Long Nam",
                    LName = "Phạm",
                    Address = "789 Pine St",
                    Email = "pln@gmail.com",
                    PhoneNumber = "1234876521",
                    IsDeleted = false,
                    TotalMoneySpent = 100.00M,
                    FortuneChance = 10,
                },
                new Customer
                {
                    Id = new Guid("8da727e0-9529-49cf-a971-945791782208"),
                    UserName = "Customer4",
                    FName = "Phát Quang",
                    LName = "Huỳnh",
                    Address = "321 Maple St",
                    Email = "hpq@gmail.com",
                    PhoneNumber = "3218764521",
                    IsDeleted = false,
                    TotalMoneySpent = 300.00M,
                    FortuneChance = 8,
                }
            };

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
                    IsDeleted = false, VehicleCapacity = 1000},
                new Shipper { Id = Guid.NewGuid(),
                    UserName = "Shipper2",
                    FName = "Bích Diễm",
                    LName = "Vũ",
                    Email = "bdv@gmail.com",
                    Address = "456 Vũ St",
                    PhoneNumber = "2345678901",
                    IsDeleted = false, VehicleCapacity = 0 },
                new Shipper { Id = Guid.NewGuid(),
                    UserName = "Shipper3",
                    FName = "Tiến Vĩ",
                    LName = "Đặng",
                    Email = "hld@gmail.com",
                    Address = "789 Đặng St",
                    PhoneNumber = "3456789012",
                    IsDeleted = false, VehicleCapacity = 1200},
                new Shipper { Id = Guid.NewGuid(),
                    UserName = "Shipper4",
                    FName = "Tiến Dụng",
                    LName = "Bùi",
                    Email = "hnb@gmail.com",
                    Address = "321 Bùi St",
                    PhoneNumber = "4567890123",
                    IsDeleted = false, VehicleCapacity = 2200},
                new Shipper { Id = Guid.NewGuid(),
                    UserName = "Shipper5",
                    FName = "Văn Hào",
                    LName = "Đỗ",
                    Email = "ptd@gmail.com",
                    Address = "654 Đỗ St",
                    PhoneNumber = "5678901234",
                    IsDeleted = false, VehicleCapacity = 3000}
            };

            // Seed Stores
            var stores = new Store[]
            {
                new Store (Guid.NewGuid()) { Name = "Store One", OpeningDate = new DateTime(2020, 1, 1).ToUniversalTime(), ContactInfo = "contact1@store.com", Location = "23 Pasteur, Phường Bến Nghé, Quận 1, Thành phố Hồ Chí Minh" },
                new Store (Guid.NewGuid()) { Name = "Store Two", OpeningDate = new DateTime(2020, 2, 1).ToUniversalTime(), ContactInfo = "contact2@store.com", Location = "88 Tô Hiến Thành, Phường 15, Quận 10, Thành phố Hồ Chí Minh" },
                new Store (Guid.NewGuid()) { Name = "Store Three", OpeningDate = new DateTime(2020, 3, 1).ToUniversalTime(), ContactInfo = "contact3@store.com", Location = "102 Dương Bá Trạc, Phường 1, Quận 8, Thành phố Hồ Chí Minh" },
                new Store (Guid.NewGuid()) { Name = "Store Four", OpeningDate = new DateTime(2020, 4, 1).ToUniversalTime(), ContactInfo = "contact4@store.com", Location = "98 Võ Văn Tần, Phường 6, Quận 3, Thành phố Hồ Chí Minh" },
                new Store (Guid.NewGuid()) { Name = "Store Five", OpeningDate = new DateTime(2020, 5, 1).ToUniversalTime(), ContactInfo = "contact5@store.com", Location = "45 Điện Biên Phủ, Phường 15, Quận Bình Thạnh, Thành phố Hồ Chí Minh", }
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
                var customerPasswords = new string[]
                {
                    "customer1", "customer2", "customer3", "customer4"
                };
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


            // Seed Products
            var products = new Product[]
            {
                // Pork
                new Product(Guid.NewGuid()) { Category = "Pork", PName = "Pork Belly", Price = 15.99, Weight = 1000, ImageURL = "pork_belly_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Pork", PName = "Pork Loin", Price = 12.99, Weight = 1000, ImageURL = "pork_loin_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Pork", PName = "Pork Chops", Price = 13.49, Weight = 800, ImageURL = "pork_chops_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Pork", PName = "Pork Ribs", Price = 18.99, Weight = 1200, ImageURL = "pork_ribs_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Pork", PName = "Ground Pork", Price = 10.99, Weight = 500, ImageURL = "ground_pork_url", IsDeleted = false },

                // Beef
                new Product(Guid.NewGuid()) { Category = "Beef", PName = "Ground Beef", Price = 11.99, Weight = 500, ImageURL = "ground_beef_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Beef", PName = "Beef Brisket", Price = 20.99, Weight = 1200, ImageURL = "beef_brisket_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Beef", PName = "Beef Ribeye", Price = 24.99, Weight = 1000, ImageURL = "beef_ribeye_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Beef", PName = "Beef Tenderloin", Price = 35.99, Weight = 800, ImageURL = "beef_tenderloin_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Beef", PName = "Beef Stew Meat", Price = 10.99, Weight = 500, ImageURL = "beef_stew_meat_url", IsDeleted = false },

                // Seafood
                new Product(Guid.NewGuid()) { Category = "Seafood", PName = "Salmon Fillet", Price = 25.99, Weight = 1000, ImageURL = "salmon_fillet_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Seafood", PName = "Shrimp", Price = 18.99, Weight = 500, ImageURL = "shrimp_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Seafood", PName = "Scallops", Price = 28.99, Weight = 400, ImageURL = "scallops_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Seafood", PName = "Cod", Price = 14.99, Weight = 1000, ImageURL = "cod_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Seafood", PName = "Crab Legs", Price = 39.99, Weight = 800, ImageURL = "crab_legs_url", IsDeleted = false },

                // Milk
                new Product(Guid.NewGuid()) { Category = "Milk", PName = "Whole Milk", Price = 3.99, Weight = 1000, ImageURL = "whole_milk_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Milk", PName = "Skim Milk", Price = 3.49, Weight = 1000, ImageURL = "skim_milk_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Milk", PName = "Almond Milk", Price = 4.99, Weight = 1000, ImageURL = "almond_milk_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Milk", PName = "Soy Milk", Price = 4.49, Weight = 1000, ImageURL = "soy_milk_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Milk", PName = "Coconut Milk", Price = 5.49, Weight = 1000, ImageURL = "coconut_milk_url", IsDeleted = false },

                // Spice
                new Product(Guid.NewGuid()) { Category = "Spice", PName = "Black Pepper", Price = 2.99, Weight = 100, ImageURL = "black_pepper_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Spice", PName = "Cinnamon", Price = 3.99, Weight = 100, ImageURL = "cinnamon_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Spice", PName = "Paprika", Price = 2.49, Weight = 100, ImageURL = "paprika_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Spice", PName = "Turmeric", Price = 3.49, Weight = 100, ImageURL = "turmeric_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Spice", PName = "Cumin", Price = 2.99, Weight = 100, ImageURL = "cumin_url", IsDeleted = false },

                // Vegetable
                new Product(Guid.NewGuid()) { Category = "Vegetable", PName = "Spinach", Price = 1.99, Weight = 500, ImageURL = "spinach_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Vegetable", PName = "Carrots", Price = 1.49, Weight = 500, ImageURL = "carrots_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Vegetable", PName = "Broccoli", Price = 2.49, Weight = 500, ImageURL = "broccoli_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Vegetable", PName = "Bell Peppers", Price = 2.99, Weight = 500, ImageURL = "bell_peppers_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Vegetable", PName = "Tomatoes", Price = 1.79, Weight = 500, ImageURL = "tomatoes_url", IsDeleted = false },

                // Sauce
                new Product(Guid.NewGuid()) { Category = "Sauce", PName = "Tomato Sauce", Price = 2.99, Weight = 500, ImageURL = "tomato_sauce_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Sauce", PName = "Soy Sauce", Price = 2.49, Weight = 500, ImageURL = "soy_sauce_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Sauce", PName = "Hot Sauce", Price = 3.99, Weight = 300, ImageURL = "hot_sauce_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Sauce", PName = "BBQ Sauce", Price = 4.49, Weight = 500, ImageURL = "bbq_sauce_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Sauce", PName = "Fish Sauce", Price = 3.49, Weight = 500, ImageURL = "fish_sauce_url", IsDeleted = false },

                // Fruit
                new Product(Guid.NewGuid()) { Category = "Fruit", PName = "Apples", Price = 1.99, Weight = 1000, ImageURL = "apples_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Fruit", PName = "Bananas", Price = 1.29, Weight = 1000, ImageURL = "bananas_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Fruit", PName = "Oranges", Price = 2.49, Weight = 1000, ImageURL = "oranges_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Fruit", PName = "Grapes", Price = 3.49, Weight = 500, ImageURL = "grapes_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Fruit", PName = "Strawberries", Price = 4.99, Weight = 500, ImageURL = "strawberries_url", IsDeleted = false },

                // Grain
                new Product(Guid.NewGuid()) { Category = "Grain", PName = "Rice", Price = 5.99, Weight = 1000, ImageURL = "rice_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Grain", PName = "Quinoa", Price = 7.99, Weight = 500, ImageURL = "quinoa_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Grain", PName = "Barley", Price = 4.99, Weight = 500, ImageURL = "barley_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Grain", PName = "Oats", Price = 3.99, Weight = 500, ImageURL = "oats_url", IsDeleted = false },
                new Product(Guid.NewGuid()) { Category = "Grain", PName = "Wheat Flour", Price = 2.99, Weight = 1000, ImageURL = "wheat_flour_url", IsDeleted = false }
            };

            // Seed ATs
            // Initialize the list to hold the AT entities
            var atEntities = new List<AT>();

            // Products that should be available in all stores
            var productsInAllStores = products.Take(30); // Choose the first 30 products to be available in all stores

            // Add products to all stores
            foreach (var product in productsInAllStores)
            {
                foreach (var store in stores)
                {
                    var atEntity = new AT
                    {
                        ProductID = product.Id,
                        Product = product,
                        StoreID = store.Id,
                        Store = store,
                        NumberAtStore = new Random().Next(10, 50), // Random stock quantity for each store
                        IsDeleted = false
                    };

                    // Add the AT entity to the atEntities list
                    atEntities.Add(atEntity);
                }
            }

            // Products that should be available in only 3 stores
            var productsInThreeStores = products.Skip(30).Take(15); // Choose the next 15 products for 3 stores

            // Add products to 3 random stores
            foreach (var product in productsInThreeStores)
            {
                var selectedStores = stores.OrderBy(s => Guid.NewGuid()).Take(3).ToList(); // Select 3 random stores
                foreach (var store in selectedStores)
                {
                    var atEntity = new AT
                    {
                        ProductID = product.Id,
                        Product = product,
                        StoreID = store.Id,
                        Store = store,
                        NumberAtStore = new Random().Next(5, 20), // Random stock quantity for each store
                        IsDeleted = false
                    };

                    // Add the AT entity to the atEntities list
                    atEntities.Add(atEntity);
                }
            }

            // Seed Bills
            // Generate Bill and Include data
            var random = new Random();
            var bills = new List<Bill>();

            for (int i = 0; i < 10; i++) // Generating 10 random bills
            {
                var customer = customers[random.Next(customers.Count())];
                var store = stores[random.Next(stores.Count())];
                var bill = new Bill(Guid.NewGuid())
                {
                    PaymentMethod = random.Next(0, 2) == 0 ? "Credit Card" : "Momo", // Randomly pick payment method
                    DateAndTime = DateTime.UtcNow.AddDays(-random.Next(1, 30)), // Random date within the past month
                    CustomerID = customer.Id,
                    Customer = customer,
                    StoreID = store.Id,
                    Store = store,
                    TotalPrice = 0, // Total will be calculated based on Includes
                    TotalWeight = 0, // Total weight will be calculated based on Includes
                    DeliveryStatus = (DeliveryStatus)random.Next(1, Enum.GetValues(typeof(DeliveryStatus)).Length), // Random status
                    IsDeleted = false
                };
                if (i != 9)
                {
                    var shipper = shippers[random.Next(shippers.Count())];
                    bill.ShipperID = shipper.Id;
                    bill.Shipper = shipper;
                }
                else if (i == 9)
                {
                    bill.DeliveryStatus = DeliveryStatus.Pending;
                    bill.ShipperID = null;
                    bill.Shipper = null;
                }

                // Generate Includes for each Bill
                int numberOfItems = random.Next(1, 6); // Random number of products per bill (between 1 and 5)

                double billTotalPrice = 0;
                int billTotalWeight = 0;

                var firstIndex = random.Next(products.Count());

                for (int j = 0; j < numberOfItems; j++)
                {
                    var product = products[(firstIndex + j) % products.Length];
                    int numberOfProducts = random.Next(1, 4); // Random number of products (between 1 and 3)

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

                    // Add the Include entity to the Bill's Includes collection
                    bill.Includes.Add(include);

                    // Update the bill's total price and weight
                    billTotalPrice += subTotal;
                    billTotalWeight += product.Weight * numberOfProducts;
                }

                // Assign total price and weight to the bill after Includes are added
                bill.TotalPrice = billTotalPrice;
                bill.TotalWeight = billTotalWeight;

                bills.Add(bill);
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
                    EndDay = new DateTime(2024, 12, 30).ToUniversalTime()
               },
                new ProductPromotion(Guid.NewGuid())
                {
                    Discount = 0.10m,
                    Name = "Winter Clearance",
                    Description = "Hot stew in winter with 10% off",
                    StartDay = new DateTime(2024, 11, 01).ToUniversalTime(),
                    EndDay = new DateTime(2024, 12, 30).ToUniversalTime()
                },
                new ProductPromotion(Guid.NewGuid())
                {
                    Discount = 0.20m,
                    Name = "Holiday Special",
                    Description = "Have fun with your family with 20% off",
                    StartDay = new DateTime(2024, 11, 01).ToUniversalTime(),
                    EndDay = new DateTime(2024, 12, 30).ToUniversalTime()
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

            // For each product promotion, apply it to a random number of products
            foreach (var promotion in productPromotions)
            {
                // Select a random subset of products to apply the promotion to
                var selectedProducts = products.OrderBy(p => Guid.NewGuid()).Take(random1.Next(1, 6)).ToList(); // Random 1 to 5 products

                foreach (var product in selectedProducts)
                {
                    // Create a new PromoteProduct object to associate the ProductPromotion with a Product
                    var promoteProduct = new PromoteProduct
                    {
                        ProductId = product.Id,
                        Product = product,
                        PromotionId = promotion.Id,
                        ProductPromotion = promotion
                    };

                    promoteProducts.Add(promoteProduct); // Add the PromoteProduct to the list of PromoteProducts

                    // Optionally, you could also add it to the promotion's PromoteProducts collection if needed
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
                    EndDay = new DateTime(2024, 12, 30).ToUniversalTime(),
                    ApplyPrice = 100,
                    PromotionChance = 50
                },
                new BillPromotion(Guid.NewGuid())
                {
                    Discount = 0.05m,
                    Name = "Weekend Special",
                    Description = "5% off for bills over $50",
                    StartDay = new DateTime(2024, 11, 01).ToUniversalTime(),
                    EndDay = new DateTime(2024, 12, 30).ToUniversalTime(),
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
                    StartDay = new DateTime(2024, 01, 01).ToUniversalTime(),
                    EndDay = new DateTime(2024, 01, 30).ToUniversalTime(),
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
                    Name = "New Beef Ribeye try out",
                    Description = "25% off for Beef Ribeye",
                    StartDay = new DateTime(2024, 11, 01).ToUniversalTime(),
                    EndDay = new DateTime(2024, 11, 30).ToUniversalTime(),
                    ProductId = products[7].Id
                },
                new CustomerPromotion(Guid.NewGuid())
                {
                    Discount = 0.30m,
                    Name = "Whole Milk discount",
                    Description = "30% off on Whole Milk",
                    StartDay = new DateTime(2024, 11, 01).ToUniversalTime(),
                    EndDay = new DateTime(2024, 11, 30).ToUniversalTime(),
                    ProductId = products[15].Id
                },
                new CustomerPromotion(Guid.NewGuid())
                {
                    Discount = 0.50m,
                    Name = "Pork Belly discount",
                    Description = "50% off on Pork Belly",
                    StartDay = new DateTime(2024, 11, 01).ToUniversalTime(),
                    EndDay = new DateTime(2024, 11, 30).ToUniversalTime(),
                    ProductId = products[0].Id
                }
            };

            var promoteCustomers = new List<PromoteCustomer>();
            var random3 = new Random();

            // Assume each customer can receive 1-3 random promotions
            foreach (var customer in customers)
            {
                // Choose a random number of promotions for the customer (between 1 and 3)
                int promotionsForCustomer = random3.Next(1, 4);

                // Randomly select promotions from customerPromotions for this customer
                var selectedPromotions = customerPromotions.OrderBy(_ => Guid.NewGuid()).Take(promotionsForCustomer);

                foreach (var promotion in selectedPromotions)
                {
                    var promoteCustomer = new PromoteCustomer
                    {
                        CustomerId = customer.Id,
                        Customer = customer,
                        PromotionId = promotion.Id,
                        CustomerPromotion = promotion
                    };

                    // Add to the PromoteCustomer list and update the CustomerPromotion's collection
                    promoteCustomers.Add(promoteCustomer);
                    promotion.PromoteCustomers.Add(promoteCustomer); // Update the collection if required
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

            await _context.SaveChangesAsync();
        }
    }
}

