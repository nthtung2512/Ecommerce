/*using AutoMapper;
using FluentValidation;
using MealMate.BLL.Dtos.Customer;
using MealMate.BLL.Services;
using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.Utils.Exceptions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace MealMate.Test.CustomerService
{
    public class CustomerAppServiceTests
    {
        private readonly Mock<UserManager<Customer>> _userManagerMock;
        private readonly Mock<IValidator<ApplicationUser>> _customerValidatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CustomerAppService _service;

        public CustomerAppServiceTests()
        {
            var userStoreMock = new Mock<IUserStore<Customer>>();
            _userManagerMock = new Mock<UserManager<Customer>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _customerValidatorMock = new Mock<IValidator<ApplicationUser>>();
            _mapperMock = new Mock<IMapper>();

            _service = new CustomerAppService(_userManagerMock.Object, _customerValidatorMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCustomerDto_WhenCustomerExists()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var existingCustomer = new Customer
            {
                Id = customerId,
                FName = "John",
                LName = "Doe",
                Address = "123 Elm Street",
                PhoneNumber = "123-456-7890",
                Email = "abc@gmail.com",
                TotalMoneySpent = 10.25m,
                FortuneChance = 5
            };

            var expectedDto = new CustomerDto
            {
                CustomerID = customerId,
                Address = "123 Elm Street",
                FName = "John",
                LName = "Doe",
                CPhone = "123-456-7890",
                CEmail = "abc@gmail.com",
                TotalMoneySpent = 0,
                FortuneChance = 0
            };

            // Setup the UserManager to return the existing customer
            _userManagerMock.Setup(x => x.FindByIdAsync(customerId.ToString()))
                .ReturnsAsync(existingCustomer);

            // Setup the mapper to map the Customer to CustomerDto
            _mapperMock.Setup(x => x.Map<CustomerDto>(existingCustomer))
                .Returns(expectedDto);

            // Act
            var result = await _service.GetByIdAsync(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.CustomerID, result.CustomerID);
            Assert.Equal(expectedDto.LName, result.LName);
            Assert.Equal(expectedDto.FName, result.LName);
            Assert.Equal(expectedDto.Address, result.Address);
            Assert.Equal(expectedDto.CPhone, result.CPhone);
            Assert.Equal(expectedDto.CEmail, result.CEmail);
            Assert.Equal(expectedDto.TotalMoneySpent, result.TotalMoneySpent);
            Assert.Equal(expectedDto.FortuneChance, result.FortuneChance);
            _userManagerMock.Verify(x => x.FindByIdAsync(customerId.ToString()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowEntityNotFoundException_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            _userManagerMock.Setup(x => x.FindByIdAsync(customerId.ToString()))
                .ReturnsAsync((Customer)null); // Simulating no customer found

            // Act & Assert
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.GetByIdAsync(customerId));
            Assert.Equal("No customer found with the specified ID", exception.Message);
            _userManagerMock.Verify(x => x.FindByIdAsync(customerId.ToString()), Times.Once);
        }
    }


    *//*[Fact]
    public async Task UpdateAsync_ShouldUpdateCustomer_WhenCustomerExists()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var existingCustomer = new Customer
        {
            Id = customerId,
            FName = "OldFName",
            LName = "OldLName",
            Address = "OldAddress",
            PhoneNumber = "OldPhone",
            TotalMoneySpent = 0,
            FortuneChance = 0
        };

        var updateData = new CustomerUpdateDto
        {
            FName = "NewFName",
            LName = "NewLName",
            Address = "NewAddress",
            CPhone = "NewPhone"
        };

        _userManagerMock.Setup(x => x.FindByIdAsync(customerId.ToString()))
            .ReturnsAsync(existingCustomer);

        _customerValidatorMock.Setup(x => x.ValidateAsync(existingCustomer))
            .ReturnsAsync(new ValidationResult { IsValid = true });

        _userManagerMock.Setup(x => x.UpdateAsync(existingCustomer))
            .ReturnsAsync(IdentityResult.Success);

        _mapperMock.Setup(x => x.Map<CustomerDto>(existingCustomer))
            .Returns(new CustomerDto
            {
                FName = "NewFName",
                LName = "NewLName",
                Address = "NewAddress",
                PhoneNumber = "NewPhone"
            });

        // Act
        var result = await _service.UpdateAsync(customerId, updateData);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("NewFName", result.FName);
        Assert.Equal("NewLName", result.LName);
        Assert.Equal("NewAddress", result.Address);
        Assert.Equal("NewPhone", result.PhoneNumber);

        _userManagerMock.Verify(x => x.FindByIdAsync(customerId.ToString()), Times.Once);
        _customerValidatorMock.Verify(x => x.ValidateAsync(existingCustomer), Times.Once);
        _userManagerMock.Verify(x => x.UpdateAsync(existingCustomer), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowEntityNotFoundException_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        _userManagerMock.Setup(x => x.FindByIdAsync(customerId.ToString()))
            .ReturnsAsync((Customer)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.UpdateAsync(customerId, new CustomerUpdateDto()));
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowEntityValidationException_WhenValidationFails()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var existingCustomer = new Customer { Id = customerId };
        var updateData = new CustomerUpdateDto();

        _userManagerMock.Setup(x => x.FindByIdAsync(customerId.ToString()))
            .ReturnsAsync(existingCustomer);

        _customerValidatorMock.Setup(x => x.ValidateAsync(existingCustomer))
            .ReturnsAsync(new ValidationResult
            {
                IsValid = false,
                Errors = new List<ValidationFailure>
                {
                    new ValidationFailure("FName", "First name is required.")
                }
            });

        // Act & Assert
        await Assert.ThrowsAsync<EntityValidationException>(() => _service.UpdateAsync(customerId, updateData));
    }
}*//*
}
*/