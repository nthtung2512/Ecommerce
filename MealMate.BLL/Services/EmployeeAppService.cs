using AutoMapper;
using MealMate.BLL.Dtos.Employee;
using MealMate.BLL.IServices;
using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.Entities.Employee;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace MealMate.BLL.Services
{
    internal class EmployeeAppService : IEmployeeAppService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IPasswordHasher<StoreManager> _passwordHasher;
        private readonly IMapper _mapper;

        public EmployeeAppService(IEmployeeRepository employeeRepository, IStoreRepository storeRepository, IPasswordHasher<StoreManager> passwordHasher, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _storeRepository = storeRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<EmployeeCreationDto> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetAsync(id) ?? throw new EntityNotFoundException("Employee not found");
            return _mapper.Map<EmployeeCreationDto>(employee);
        }

        public async Task<EmployeeCreationDto> GetStoreManagerForLoginAsync(string email, string password)
        {
            var employee = await _employeeRepository.GetStoreManagerForLoginAsync(email, password) ?? throw new UnauthorizedAccessException("Invalid credentials");

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(employee, employee.Password, password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid credentials");

            return _mapper.Map<EmployeeCreationDto>(employee); ;
        }

        public async Task<EmployeeCreationDto> UpdateEmployeeAsync(Guid id, EmployeeUpdateDto updateData)
        {
            var employee = await _employeeRepository.GetAsync(id) ?? throw new EntityNotFoundException("Employee not found");

            if (updateData.EPhones != null)
            {
                employee.EPhones.Clear();
                foreach (var phone in updateData.EPhones)
                {
                    employee.EPhones.Add(new EPhone
                    {
                        EmployeeID = employee.Id,
                        EPhoneNumber = phone
                    });
                }
            }

            employee.FirstName = updateData.FirstName ?? employee.FirstName;
            employee.LastName = updateData.LastName ?? employee.LastName;
            employee.Salary = updateData.Salary ?? employee.Salary;
            employee.Address = updateData.Address ?? employee.Address;
            employee.StoreId = updateData.StoreID ?? employee.StoreId;

            await _employeeRepository.UpdateAsync(employee);

            return _mapper.Map<EmployeeCreationDto>(employee);
        }

        public async Task DeleteEmployeeAsync(Guid id)
        {
            var employee = await _employeeRepository.GetAsync(id) ?? throw new EntityNotFoundException("Employee not found");
            await _employeeRepository.DeleteAsync(employee);
        }
    }
}
