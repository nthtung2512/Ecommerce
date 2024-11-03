using AutoMapper;
using MealMate.BLL.Dtos.Bills;
using MealMate.BLL.Dtos.Customer;
using MealMate.BLL.Dtos.Employee;
using MealMate.BLL.Dtos.Product;
using MealMate.BLL.Dtos.Promotion;
using MealMate.BLL.Dtos.Shipper;
using MealMate.BLL.Dtos.Stores;
using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.Entities.Products;
using MealMate.DAL.Entities.Promotion;
using MealMate.DAL.Entities.Stores;
using MealMate.DAL.Entities.Transactions;

namespace MealMate.BLL.AutoMapperProfiles
{
    public class MealMateAutoMapperProfile : Profile
    {
        public MealMateAutoMapperProfile()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<Bill, BillCreationDto>();
            CreateMap<StoreManager, EmployeeCreationDto>();
            CreateMap<Product, ProductCreationDto>();
            CreateMap<BillPromotion, BillPromotionCreationDto>();
            CreateMap<ProductPromotion, ProductPromotionCreationDto>();
            CreateMap<Promotion, PromotionCreationDto>();
            CreateMap<Shipper, ShipperCreationDto>();
            CreateMap<AT, ATDto>();
            CreateMap<Include, IncludeCreationDto>();
            CreateMap<Store, StoreDto>();
        }
    }
}
