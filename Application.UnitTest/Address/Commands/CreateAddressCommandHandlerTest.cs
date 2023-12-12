using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Addresses.CQRS.Commands;
using Application.Features.Addresses.CQRS.Handlers;
using Application.Features.Addresses.DTOs;
using Application.Features.Addresses.DTOs.Validators;
using Application.Contracts.Persistence;
using Application.UnitTest.Mocks;
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
    public class CreateAddressCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidAddress_ReturnsSuccessWithCorrectValues()
        {
            // Arrange
            var unitOfWorkMock = MockUnitOfWork.GetUnitOfWork();
            var mapperMock = new Mock<IMapper>();

            // Set up the CreateAddressDto with sample values
            var institutionProfiles = new List<InstitutionProfile>
            {
                new InstitutionProfile
                {
                    Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                    InstitutionName = "Institution 1",
                    BranchName = "Branch 1",
                    Website = "www.Website.com",
                    PhoneNumber = "Phone 1",
                    Summary = "Summary 1",
                    EstablishedOn = DateTime.Now.AddDays(-10),
                    Rate = 4.5,
                    LogoId = "LogoId 1",
                    BannerId = "BannerId 1"
                },
                new InstitutionProfile
                {
                    Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7"),
                    InstitutionName = "Institution 2",
                    BranchName = "Branch 2",
                    Website = "www.Website.com",
                    PhoneNumber = "Phone 2",
                    Summary = "Summary 2",
                    EstablishedOn = DateTime.Now.AddDays(-10),
                    Rate = 3.8,
                    LogoId = "LogoId 2",
                    BannerId = "BannerId 2"
                }
            };

            var createAddressDto = new CreateAddressDto
            {
                Country = "Sample Country",
                Region = "Sample Region",
                Zone = "Sample Zone",
                Woreda = "Sample Woreda",
                City = "Sample City",
                SubCity = "Sample SubCity",
                Longitude = 1.23,
                Latitude = 4.56,
                Summary = "Sample Summary",
                InstitutionId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6")
            };

            // Set up the CreateAddressCommand with the CreateAddressDto
            var createAddressCommand = new CreateAddressCommand
            {
                CreateAddressDto = createAddressDto
            };

            // Set up the Address entity
            var addressEntity = new Address
            {
                Id = Guid.NewGuid()
            };

            // Set up the mock dependencies
            mapperMock.Setup(m => m.Map<Address>(createAddressDto)).Returns(addressEntity);

            // Create an instance of the CreateAddressCommandHandler with the mock dependencies
            var handler = new CreateAddressCommandHandler(unitOfWorkMock.Object, mapperMock.Object);

            // Act
            var result = await handler.Handle(createAddressCommand, CancellationToken.None);

           // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Value.ShouldBe(addressEntity.Id);
            unitOfWorkMock.Verify(x => x.Save(), Times.Exactly(1));
        }

        [Fact]
        public async Task Handle_InvalidAddress_ReturnsFailureWithErrorMessage()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            // Set up an invalid CreateAddressDto without required properties
            var createAddressDto = new CreateAddressDto();

            // Set up the CreateAddressCommand with the invalid CreateAddressDto
            var createAddressCommand = new CreateAddressCommand
            {
                CreateAddressDto = createAddressDto
            };

            // Set up the validator to return validation errors
            var validatorMock = new Mock<IValidator<CreateAddressDto>>();
            validatorMock
                .Setup(v => v.ValidateAsync(createAddressDto, CancellationToken.None))
                .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Country", "Country is required") }));

            // Create an instance of the CreateAddressCommandHandler with the mock dependencies
            var handler = new CreateAddressCommandHandler(unitOfWorkMock.Object, mapperMock.Object);

            // Act
            var result = await handler.Handle(createAddressCommand, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldBe("Country is required.");
            unitOfWorkMock.Verify(u => u.AddressRepository.Add(It.IsAny<Address>()), Times.Never);
            unitOfWorkMock.Verify(u => u.Save(), Times.Never);
            mapperMock.Verify(m => m.Map<Address>(createAddressDto), Times.Never);
        }
    }
}
