using AutoMapper;
using FluentValidation;
using MealMate.BLL.Dtos.Customer;
using MealMate.BLL.IServices;
using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils.Exceptions;

namespace MealMate.BLL.Services
{
    public class CustomerAppService : ICustomerAppService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IValidator<ApplicationUser> _customerValidator;
        private readonly IMapper _mapper;

        public CustomerAppService(IValidator<ApplicationUser> customerValidator, IMapper mapper, ICustomerRepository customerRepository)
        {
            _customerValidator = customerValidator;
            _mapper = mapper;
            _customerRepository = customerRepository;
        }

        private CustomerDto Map(Customer customer) => _mapper.Map<CustomerDto>(customer);

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
            var existingCustomer = await _customerRepository.GetAsync(customerId) ?? throw new EntityNotFoundException("No customer found");

            // Update customer properties
            existingCustomer.Address = updateData.Address ?? existingCustomer.Address;
            existingCustomer.FName = updateData.FName ?? existingCustomer.FName;
            existingCustomer.LName = updateData.LName ?? existingCustomer.LName;
            existingCustomer.PhoneNumber = updateData.CPhone ?? existingCustomer.PhoneNumber;

            var validationResult = await _customerValidator.ValidateAsync(existingCustomer);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException(
                    $"Validation exception when updating customer: {string.Join(", ", validationResult.Errors)}"
                );
            }

            await _customerRepository.UpdateAsync(existingCustomer);

            return Map(existingCustomer);
        }

        // Get the last customer ID
        public async Task<Guid> GetLastCustomerIdAsync()
        {
            var lastUser = await _customerRepository.GetLastCustomerIdAsync();
            return lastUser;
        }

        public async Task<CustomerDto> AddTotalMoneySpentByIdAsync(Guid id, decimal money)
        {
            // Fetch customer by ID to update
            var existingCustomer = await _customerRepository.GetAsync(id) ?? throw new EntityNotFoundException("No customer found");

            // Update customer properties
            existingCustomer.FortuneChance += (int)(money / 100);
            existingCustomer.FortuneChance += (int)((money % 100.00m + existingCustomer.TotalMoneySpent % 100.00m) / 100);

            existingCustomer.TotalMoneySpent += money;

            await _customerRepository.UpdateAsync(existingCustomer);

            return Map(existingCustomer);
        }
    }

}
