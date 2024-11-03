using AutoMapper;
using FluentValidation;
using MealMate.BLL.Dtos.Customer;
using MealMate.BLL.IServices;
using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils.Exceptions;
using MealMate.DAL.Utils.GuidUtil;
using Microsoft.AspNetCore.Identity;

namespace MealMate.BLL.Services
{
    internal class CustomerAppService : ICustomerAppService
    {
        private readonly IPasswordHasher<Customer> _passwordHasher;
        private readonly GuidGenerator _guidGenerator;
        private readonly IValidator<Customer> _customerValidator;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerAppService(IPasswordHasher<Customer> passwordHasher, GuidGenerator guidGenerator, ICustomerRepository customerRepository, IValidator<Customer> customerValidator, IMapper mapper)
        {
            _passwordHasher = passwordHasher;
            _guidGenerator = guidGenerator;
            _customerRepository = customerRepository;
            _customerValidator = customerValidator;
            _mapper = mapper;
        }

        private CustomerDto Map(Customer customer) => _mapper.Map<CustomerDto>(customer);

        public async Task<CustomerDto> CreateAsync(CustomerCreationDto createData)
        {
            Customer insertingCustomer = new(_guidGenerator.Create())
            {
                CAddress = createData.CAddress,
                CFName = createData.CFName,
                CLName = createData.CLName,
                CPhone = createData.CPhone,
                CEmail = createData.CEmail,
                Password = createData.Password,
                TotalMoneySpent = 0.0m,
                IsDeleted = false
            };

            insertingCustomer.Password = _passwordHasher.HashPassword(insertingCustomer, createData.Password);

            var validationResult = await _customerValidator.ValidateAsync(insertingCustomer);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException(
                    $"Validation exception when create new customer: {string.Join(", ", validationResult.Errors)}"
                );
            }

            await _customerRepository.CreateAsync(insertingCustomer);

            return Map(insertingCustomer);
        }

        // Get customer list
        public async Task<List<CustomerDto>> GetListAsync()
        {
            // Fetch list of customers from the repository
            var customersList = await _customerRepository.GetCustomersListAsync();
            if (customersList.Count == 0)
            {
                throw new EntityNotFoundException("No customers found");
            }
            return customersList.Select(Map).ToList();
        }

        // Get customer by ID
        public async Task<CustomerDto> GetByIdAsync(Guid customerId)
        {
            // Fetch customer by ID from the repository
            var result = await _customerRepository.GetAsync(customerId) ?? throw new EntityNotFoundException("No customer found");
            return Map(result);
        }

        // Update customer by ID
        public async Task<CustomerDto> UpdateAsync(Guid customerId, CustomerUpdateDto updateData)
        {
            // Fetch customer by ID to update
            var existingCustomer = await _customerRepository.GetAsync(customerId) ?? throw new EntityNotFoundException("No customer found");

            // Update customer properties
            existingCustomer.CAddress = updateData.CAddress ?? existingCustomer.CAddress;
            existingCustomer.CFName = updateData.CFName ?? existingCustomer.CFName;
            existingCustomer.CLName = updateData.CLName ?? existingCustomer.CLName;
            existingCustomer.CPhone = updateData.CPhone ?? existingCustomer.CPhone;

            var validationResult = await _customerValidator.ValidateAsync(existingCustomer);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException(
                    $"Validation exception when update customer: {string.Join(", ", validationResult.Errors)}"
                );
            }

            await _customerRepository.UpdateAsync(existingCustomer);

            return Map(existingCustomer);
        }

        // Delete customer by ID
        public async Task DeleteAsync(Guid customerId)
        {
            var customer = await _customerRepository.GetAsync(customerId) ?? throw new EntityNotFoundException("Customer not found");

            await _customerRepository.DeleteAsync(customer);
        }

        // TODO: Move to auth later. Get customer for login based on email and password
        public async Task<CustomerDto> GetCustomerForLoginAsync(string email, string password)
        {
            var customer = await _customerRepository.GetCustomerForLoginAsync(email, password) ?? throw new UnauthorizedAccessException("Invalid credentials");

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(customer, customer.Password, password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid credentials");

            return Map(customer);
        }

        // Get the last customer ID
        public async Task<Guid> GetLastCustomerIdAsync()
        {
            return await _customerRepository.GetLastCustomerIdAsync();
        }
        public async Task<CustomerDto> AddTotalMoneySpentByIdAsync(Guid id, decimal money)
        {
            // Fetch customer by ID to update
            var existingCustomer = await _customerRepository.GetAsync(id) ?? throw new EntityNotFoundException("No customer found");

            // Update customer properties
            existingCustomer.TotalMoneySpent += money;

            await _customerRepository.UpdateAsync(existingCustomer);

            return Map(existingCustomer);
        }
    }

}
