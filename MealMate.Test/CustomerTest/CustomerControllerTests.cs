using MealMate.BLL.Dtos.Customer;
using MealMate.BLL.IServices;
using MealMate.DAL.Entities.ApplicationUser;
using MealMate.PL.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MealMate.Test.CustomerTest
{
    public class CustomerControllerTests
    {
        private readonly Customer customer = new(Guid.NewGuid())
        {
            CAddress = "1234 Main St",
            CFName = "John",
            CLName = "Doe",
            CPhone = "1234567890",
            CEmail = "abc@gmail.com",
            TotalMoneySpent = 0.0m,
            Password = "password",
            IsDeleted = false
        };
        private readonly Mock<ICustomerAppService> _mockCustomerAppService;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _mockCustomerAppService = new Mock<ICustomerAppService>();
            _controller = new CustomerController(_mockCustomerAppService.Object);
        }

        [Fact]
        public async Task GetCustomerList_ReturnsOkResult_WithCustomerList()
        {
            // Arrange
            var customers = new List<CustomerDto>
            {
                new() { CustomerID = Guid.NewGuid(), CAddress = "1234 Main St", CFName = "John", CLName = "Doe", CPhone = "1234567890", CEmail = "abc@gmail.com" },
                new() { CustomerID = Guid.NewGuid(), CAddress = "5678 Elm St", CFName = "Jane", CLName = "Smith", CPhone = "9876543210", CEmail = "jane.smith@example.com" },
                new() { CustomerID = Guid.NewGuid(), CAddress = "9101 Oak St", CFName = "Bob", CLName = "Brown", CPhone = "5551234567", CEmail = "bob.brown@example.com" }
            };

            _mockCustomerAppService.Setup(service => service.GetListAsync()).ReturnsAsync(customers);

            // Act
            var result = await _controller.GetCustomerList();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(customers, okResult.Value);
        }

        [Fact]
        public async Task GetCustomerById_ReturnsOkResult_WithCustomer()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = new CustomerDto()
            {
                CustomerID = Guid.NewGuid(),
                CAddress = "1234 Main St",
                CFName = "John",
                CLName = "Doe",
                CPhone = "1234567890",
                CEmail = "abc@gmail.com"
            };

            _mockCustomerAppService.Setup(service => service.GetByIdAsync(customerId)).ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCustomerById(customerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(customer, okResult.Value);
        }

        [Fact]
        public async Task UpdateCustomerInfoById_ReturnsOkResult_WithUpdatedCustomer()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customerUpdate = new CustomerUpdateDto()
            {
                CAddress = "5 Main St",
                CFName = "J",
                CLName = "D",
                CPhone = "123123123"
            };
            var updatedCustomer = new CustomerDto()
            {
                CustomerID = customerId,
                CAddress = "5 Main St",
                CFName = "J",
                CLName = "D",
                CPhone = "123123123",
                CEmail = "abc@gmail.com"
            };

            _mockCustomerAppService.Setup(service => service.UpdateAsync(customerId, customerUpdate)).ReturnsAsync(updatedCustomer);

            // Act
            var result = await _controller.UpdateCustomerInfoById(customerId, customerUpdate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(updatedCustomer, okResult.Value);
        }

        [Fact]
        public async Task CreateCustomer_ReturnsOkResult_WithCreatedCustomer()
        {
            // Arrange
            var id = Guid.NewGuid();
            var customerCreate = new CustomerCreationDto()
            {
                CustomerID = id,
                CAddress = "1234 Main St",
                CFName = "John",
                CLName = "Doe",
                CPhone = "1234567890",
                CEmail = "abc@gmail.com",
                Password = "password",
                TotalMoneySpent = 0.0m
            };

            var createdCustomer = new CustomerDto
            {
                CustomerID = id,
                CAddress = "1234 Main St",
                CFName = "John",
                CLName = "Doe",
                CPhone = "1234567890",
                CEmail = "abc@gmail.com",
                TotalMoneySpent = 0.0m
            };

            _mockCustomerAppService.Setup(service => service.CreateAsync(customerCreate)).ReturnsAsync(createdCustomer);

            // Act
            var result = await _controller.CreateCustomer(customerCreate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(createdCustomer, okResult.Value);
        }

        [Fact]
        public async Task GetCustomerForLogin_ReturnsOkResult_WithCustomer()
        {
            // Arrange
            var email = "abc@gmail.com";
            var password = "password";

            var customer = new CustomerDto
            {
                CustomerID = Guid.NewGuid(),
                CAddress = "1234 Main St",
                CFName = "John",
                CLName = "Doe",
                CPhone = "1234567890",
                CEmail = "abc@gmail.com",
                TotalMoneySpent = 0.0m
            };

            _mockCustomerAppService.Setup(service => service.GetCustomerForLoginAsync(email, password)).ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCustomerForLogin(email, password);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(customer, okResult.Value);
        }

        [Fact]
        public async Task GetCustomerRank_ReturnsOkResult_WithCorrectRank()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = new CustomerDto
            {
                CustomerID = customerId,
                CAddress = "1234 Main St",
                CFName = "John",
                CLName = "Doe",
                CPhone = "1234567890",
                CEmail = "abc@gmail.com",
                TotalMoneySpent = 0.0m
            };

            _mockCustomerAppService.Setup(service => service.GetByIdAsync(customerId)).ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCustomerRank(customerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("platinum", ((dynamic)okResult.Value).rank);
        }

        [Fact]
        public async Task GetLastCustomerID_ReturnsOkResult_WithLastCustomerID()
        {
            // Arrange
            var lastCustomerId = Guid.NewGuid();
            _mockCustomerAppService.Setup(service => service.GetLastCustomerIdAsync()).ReturnsAsync(lastCustomerId);

            // Act
            var result = await _controller.GetLastCustomerID();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(lastCustomerId, okResult.Value);
        }
    }
}
