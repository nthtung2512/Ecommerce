using AutoMapper;
using MealMate.BLL.Dtos.Employee;
using MealMate.BLL.IServices;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils.Exceptions;

namespace MealMate.BLL.Services
{
    internal class EmployeeAppService : IEmployeeAppService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeAppService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDto>> GetListEmployeeAsync()
        {
            var employees = await _employeeRepository.GetStoreManagerListAsync();
            if (employees.Count == 0)
                throw new EntityNotFoundException("Employees not found");
            return _mapper.Map<List<EmployeeDto>>(employees);
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetAsync(id) ?? throw new EntityNotFoundException("Employee not found");
            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto> GetEmployeeByStoreIdAsync(Guid storeid)
        {
            var employee = await _employeeRepository.GetEmployeeByStoreIdAsync(storeid) ?? throw new EntityNotFoundException("Employee not found");
            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto> UpdateEmployeeAsync(Guid id, EmployeeUpdateDto updateData)
        {
            var employee = await _employeeRepository.GetAsync(id) ?? throw new EntityNotFoundException("Employee not found");

            employee.FName = updateData.FName ?? employee.FName;
            employee.LName = updateData.LName ?? employee.LName;
            employee.Salary = updateData.Salary ?? employee.Salary;
            employee.Address = updateData.Address ?? employee.Address;
            employee.PhoneNumber = updateData.Phone ?? employee.PhoneNumber;

            await _employeeRepository.UpdateAsync(employee);

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task DeleteEmployeeAsync(Guid id)
        {
            // Fetch the employee by ID
            var employee = await _employeeRepository.GetAsync(id)
                ?? throw new EntityNotFoundException("Employee not found");

            // Update the employee to save the changes
            await _employeeRepository.DeleteAsync(employee);
        }
    }
}
