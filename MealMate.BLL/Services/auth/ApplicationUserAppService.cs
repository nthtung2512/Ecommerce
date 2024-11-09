using AutoMapper;
using FluentValidation;
using MealMate.BLL.Dtos.ApplicationUser;
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
            var user = new Customer()
            {
                Id = _guidGenerator.Create(),
                Email = customerDto.CEmail,
                UserName = customerDto.FName + customerDto.LName,
                FName = customerDto.FName,
                LName = customerDto.LName,
                Address = customerDto.Address ?? string.Empty,
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

        public async Task<EmployeeDto> RegisterStoreManagerAsync(EmployeeCreationDto storeManagerDto)
        {
            var user = new StoreManager()
            {
                Id = _guidGenerator.Create(),
                Email = storeManagerDto.Email,
                UserName = storeManagerDto.FName + storeManagerDto.LName,
                FName = storeManagerDto.FName,
                LName = storeManagerDto.LName,
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

            await _userManager.AddToRoleAsync(user, "StoreManager");

            return _mapper.Map<EmployeeDto>(user);
        }

        public async Task<ShipperDto> RegisterShipperAsync(ShipperCreationDto shipperDto)
        {
            var user = new Shipper()
            {
                Id = _guidGenerator.Create(),
                Email = shipperDto.SEmail,
                UserName = shipperDto.FName + shipperDto.LName,
                FName = shipperDto.FName,
                LName = shipperDto.LName,
                Address = shipperDto.Address ?? string.Empty,
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

        public async Task<UserWithRoleDto> GetUserAsync(string email, string password)
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
            UserWithRoleDto userWithRoleData;

            if (role == "Customer")
            {
                var customer = await _customerRepository.GetAsync(user.Id);
                userWithRoleData = new UserWithRoleDto
                {
                    User = _mapper.Map<CustomerDto>(customer),
                    Role = role
                };
            }
            else if (role == "StoreManager")
            {
                var storeManager = await _employeeRepository.GetAsync(user.Id);
                userWithRoleData = new UserWithRoleDto
                {
                    User = _mapper.Map<EmployeeDto>(storeManager),
                    Role = role
                };
            }
            else if (role == "Shipper")
            {
                var shipper = await _shipperRepository.GetAsync(user.Id);
                userWithRoleData = new UserWithRoleDto
                {
                    User = _mapper.Map<ShipperDto>(shipper),
                    Role = role
                };
            }
            else if (role == "Admin")
            {
                userWithRoleData = new UserWithRoleDto
                {
                    User = _mapper.Map<ApplicationUserDto>(user),
                    Role = role
                };
            }
            else
            {
                throw new EntityValidationException("Invalid user role");
            }

            return userWithRoleData;
        }
    }

}
