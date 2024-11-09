using AutoMapper;
using MealMate.BLL.Dtos.ApplicationUser;
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
            CreateMap<StoreManager, EmployeeDto>();
            CreateMap<Shipper, ShipperDto>();
            CreateMap<ApplicationUser, ApplicationUserDto>();
            CreateMap<Bill, BillDto>()
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.Id));
            CreateMap<Product, ProductCreationDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id));
            CreateMap<BillPromotion, BillPromotionCreationDto>()
                .ForMember(dest => dest.PromotionId, opt => opt.MapFrom(src => src.Id));
            CreateMap<AT, ATDto>();
            CreateMap<Store, StoreDto>()
                .ForMember(dest => dest.StoreID, opt => opt.MapFrom(src => src.Id));
        }
    }
}
