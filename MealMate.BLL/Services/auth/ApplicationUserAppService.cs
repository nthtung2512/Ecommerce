using AutoMapper;
using FluentValidation;
using MealMate.BLL.Dtos.Customer;
using MealMate.BLL.Dtos.Employee;
using MealMate.BLL.Dtos.Shipper;
using MealMate.BLL.IServices.auth;
using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils.Exceptions;
using MealMate.DAL.Utils.GuidUtil;
using Microsoft.AspNetCore.Identity;

namespace MealMate.BLL.Services.auth
{
    internal class ApplicationUserAppService : IApplicationUserAppService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IShipperRepository _shipperRepository;
        private readonly IValidator<ApplicationUser> _userValidator;
        private readonly GuidGenerator _guidGenerator;
        private readonly IMapper _mapper;

        public ApplicationUserAppService(UserManager<ApplicationUser> userManager, IValidator<ApplicationUser> userValidator, GuidGenerator guidGenerator, IMapper mapper, ICustomerRepository customerRepository, IEmployeeRepository employeeRepository, IShipperRepository shipperRepository)
        {
            _userManager = userManager;
            _userValidator = userValidator;
            _guidGenerator = guidGenerator;
            _mapper = mapper;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _shipperRepository = shipperRepository;
        }

        public async Task<CustomerDto> RegisterCustomerAsync(CustomerCreationDto customerDto)
        {
            var user = new Customer(_guidGenerator.Create())
            {
                Email = customerDto.CEmail,
                FirstName = customerDto.CFName,
                LastName = customerDto.CLName,
                Address = customerDto.CAddress ?? string.Empty,
                PhoneNumber = customerDto.CPhone,
            };

            var validationResult = await _userValidator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException(
                    $"Validation exception when create new customer: {string.Join(", ", validationResult.Errors)}"
                );
            }
            // Create the user and hash the password
            var result = await _userManager.CreateAsync(user, customerDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new EntityBadRequestException($"Failed to create new customer: {errors}");
            }

            await _userManager.AddToRoleAsync(user, "Customer");

            return _mapper.Map<CustomerDto>(user);
        }

        public async Task<EmployeeCreationDto> RegisterStoreManagerAsync(EmployeeCreationDto storeManagerDto)
        {
            var user = new StoreManager(_guidGenerator.Create())
            {
                Email = storeManagerDto.Email,
                FirstName = storeManagerDto.FirstName,
                LastName = storeManagerDto.LastName,
                Address = storeManagerDto.Address ?? string.Empty,
                PhoneNumber = storeManagerDto.EPhone,
                Salary = storeManagerDto.Salary,
                StoreId = storeManagerDto.StoreID
            };

            var validationResult = await _userValidator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException(
                    $"Validation exception when create new manager: {string.Join(", ", validationResult.Errors)}"
                );
            }
            var result = await _userManager.CreateAsync(user, storeManagerDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new EntityBadRequestException($"Failed to create new manager: {errors}");
            }

            await _userManager.AddToRoleAsync(user, "Manager");

            return _mapper.Map<EmployeeCreationDto>(user);
        }

        public async Task<ShipperDto> RegisterShipperAsync(ShipperCreationDto shipperDto)
        {
            var user = new Shipper(_guidGenerator.Create())
            {
                Email = shipperDto.SEmail,
                FirstName = shipperDto.SFName,
                LastName = shipperDto.SLName,
                Address = shipperDto.SAddress ?? string.Empty,
                PhoneNumber = shipperDto.SPhoneNo,
            };

            var validationResult = await _userValidator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException(
                    $"Validation exception when create new shipper: {string.Join(", ", validationResult.Errors)}"
                );
            }
            var result = await _userManager.CreateAsync(user, shipperDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new EntityBadRequestException($"Failed to create new shipper: {errors}");
            }

            await _userManager.AddToRoleAsync(user, "Shipper");

            return _mapper.Map<ShipperDto>(user);
        }

        public async Task<ApplicationUser> GetUserAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                throw new EntityValidationException("Invalid email or password");
            }

            // Retrieve user roles
            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || roles.Count == 0)
            {
                throw new EntityBadRequestException("User has no role");
            }

            // Assuming a user can only have one role
            string role = roles.First();

            // Fetch role-specific data based on role
            return role switch
            {
                "Customer" => await _customerRepository.GetAsync(user.Id),
                "StoreManager" => await _employeeRepository.GetAsync(user.Id),
                "Shipper" => await _shipperRepository.GetAsync(user.Id),
                "Admin" => user,
                _ => throw new EntityValidationException("Invalid user role")
            };
        }
    }

}
