using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Addresses.CQRS.Commands;
using Application.Features.Addresses.CQRS.Handlers;
using Application.Features.Addresses.DTOs;
using Application.Features.Addresses.DTOs.Validators;
using Application.Contracts.Persistence;
using Application.Responses;
using AutoMapper;
using Domain;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using Shouldly;
using Xunit;


namespace Application.UnitTest.Addresses.Commands
{
    public class UpdateAddressCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidAddress_ReturnsSuccess()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            // Set up the UpdateAddressDto with sample values
            var updateAddressDto = new UpdateAddressDto
            {
                Id = Guid.NewGuid(), // Provide an existing ID
                Country = "Updated Country",
                Region = "Updated Region",
                Zone = "Updated Zone",
                Woreda = "Updated Woreda",
                City = "Updated City",
                SubCity = "Updated SubCity",
                Longitude = 1.23,
                Latitude = 4.56,
                Summary = "Updated Summary",
                InstitutionId = Guid.NewGuid()
            };

            // Set up the UpdateAddressCommand with the UpdateAddressDto
            var updateAddressCommand = new UpdateAddressCommand
            {
                UpdateAddressDto = updateAddressDto
            };

            // Set up the Address entity
            var addressEntity = new Address
            {
                Id = updateAddressDto.Id,
                // Set other properties based on updateAddressDto
            };

            // Set up the expected result
            var expectedResult = Result<Unit>.Success(Unit.Value);

            // Set up the mock dependencies
            unitOfWorkMock.Setup(u => u.AddressRepository.Get(updateAddressDto.Id)).ReturnsAsync(addressEntity);
            unitOfWorkMock.Setup(u => u.AddressRepository.Update(It.IsAny<Address>()));
            unitOfWorkMock.Setup(u => u.Save()).ReturnsAsync(1);
            mapperMock.Setup(m => m.Map(updateAddressDto, addressEntity)).Returns(addressEntity);

            // Create an instance of the UpdateAddressCommandHandler with the mock dependencies
            var handler = new UpdateAddressCommandHandler(unitOfWorkMock.Object, mapperMock.Object);

            // Act
            var result = await handler.Handle(updateAddressCommand, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBe(expectedResult.IsSuccess);
            unitOfWorkMock.Verify(u => u.AddressRepository.Get(updateAddressDto.Id), Times.Once);
            unitOfWorkMock.Verify(u => u.AddressRepository.Update(It.IsAny<Address>()), Times.Once);
            unitOfWorkMock.Verify(u => u.Save(), Times.Once);
            mapperMock.Verify(m => m.Map(updateAddressDto, addressEntity), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingAddress_ReturnsFailure()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            // Set up the UpdateAddressDto with sample values
            var updateAddressDto = new UpdateAddressDto
            {
                Id = Guid.NewGuid(), // Provide a non-existing ID
                Country = "Updated Country",
                Region = "Updated Region",
                Zone = "Updated Zone",
                Woreda = "Updated Woreda",
                City = "Updated City",
                SubCity = "Updated SubCity",
                Longitude = 1.23,
                Latitude = 4.56,
                Summary = "Updated Summary",
                InstitutionId = Guid.NewGuid()
            };

            // Set up the UpdateAddressCommand with the UpdateAddressDto
            var updateAddressCommand = new UpdateAddressCommand
            {
                UpdateAddressDto = updateAddressDto
            };

            // Set up the expected result
            var expectedResult = Result<Unit>.Failure("Update Failed");

            // Set up the mock dependency to return null for non-existing address
            unitOfWorkMock.Setup(u => u.AddressRepository.Get(updateAddressDto.Id)).ReturnsAsync((Address)null);

            // Create an instance of the UpdateAddressCommandHandler with the mock dependencies
            var handler = new UpdateAddressCommandHandler(unitOfWorkMock.Object, mapperMock.Object);

            // Act
            var result = await handler.Handle(updateAddressCommand, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBe(expectedResult.IsSuccess);
            result.Error.ShouldBe(expectedResult.Error);
            unitOfWorkMock.Verify(u => u.AddressRepository.Get(updateAddressDto.Id), Times.Once);
            unitOfWorkMock.Verify(u => u.AddressRepository.Update(It.IsAny<Address>()), Times.Never);
            unitOfWorkMock.Verify(u => u.Save(), Times.Never);
            mapperMock.Verify(m => m.Map(updateAddressDto, It.IsAny<Address>()), Times.Never);
        }

        [Fact]
        public async Task Handle_InvalidAddress_ReturnsFailure()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            // Set up the UpdateAddressDto with sample values
            var updateAddressDto = new UpdateAddressDto
            {
                Id = Guid.NewGuid(), // Provide an existing ID
                // Missing required properties to make the address invalid
            };

            // Set up the UpdateAddressCommand with the UpdateAddressDto
            var updateAddressCommand = new UpdateAddressCommand
            {
                UpdateAddressDto = updateAddressDto
            };

            // Set up the validation failure
            var validationFailure = new ValidationFailure("Country", "Country is required.");
            var validationErrors = new ValidationFailure[] { validationFailure };
            var validationResult = new ValidationResult(validationErrors);

            // Set up the expected result
            var expectedResult = Result<Unit>.Failure(validationFailure.ErrorMessage);

            // Set up the mock validator to return the validation result
            var validatorMock = new Mock<IValidator<UpdateAddressDto>>();
            validatorMock.Setup(v => v.ValidateAsync(updateAddressDto, CancellationToken.None)).ReturnsAsync(validationResult);

            // Create an instance of the UpdateAddressCommandHandler with the mock dependencies
            var handler = new UpdateAddressCommandHandler(unitOfWorkMock.Object, mapperMock.Object);
            

            // Act
            var result = await handler.Handle(updateAddressCommand, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBe(expectedResult.IsSuccess);
            result.Error.ShouldBe(expectedResult.Error);
            unitOfWorkMock.Verify(u => u.AddressRepository.Get(updateAddressDto.Id), Times.Never);
            unitOfWorkMock.Verify(u => u.AddressRepository.Update(It.IsAny<Address>()), Times.Never);
            unitOfWorkMock.Verify(u => u.Save(), Times.Never);
            mapperMock.Verify(m => m.Map(updateAddressDto, It.IsAny<Address>()), Times.Never);
        }
    }
}
