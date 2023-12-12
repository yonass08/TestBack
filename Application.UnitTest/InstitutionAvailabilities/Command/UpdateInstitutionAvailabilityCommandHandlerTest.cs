using Application.Contracts.Persistence;
using Application.UnitTest.Mocks;
using Application.UnitTest.Mocks;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Threading;
using Application.Features.InstitutionAvailabilities.CQRS.Commands;
using Application.Features.InstitutionAvailabilities.CQRS.Handlers;
using Application.Features.InstitutionAvailabilities.DTOs;
using Application.Features.InstitutionAvailabilities.DTOs.Validators;
using Application.Responses;
using AutoMapper;
using Domain;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace Application.UnitTest.Features.InstitutionAvailabilities.CQRS.Handlers
{
    public class UpdateInstitutionAvailabilityCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateInstitutionAvailabilityCommandHandler _handler;

        public UpdateInstitutionAvailabilityCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new UpdateInstitutionAvailabilityCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsSuccessResult()
        {
            // Arrange
            var institutionAvailabilityDto = new UpdateInstitutionAvailabilityDto
            {
                Id = Guid.NewGuid(),
                  StartDay = "Monday",
                    EndDay = "Sunday",
                    Opening = "2:00AM",
                    Closing = "4:00PM",
                    TwentyFourHours = true,
                    InstitutionId = Guid.NewGuid()

            };
            var command = new UpdateInstitutionAvailabilityCommand
            {
                UpdateInstitutionAvailabilityDto = institutionAvailabilityDto
            };
            var validationResult = new UpdateInstitutionAvailabillityDtoValidator().Validate(institutionAvailabilityDto);
            _mockUnitOfWork.Setup(uow => uow.InstitutionAvailabilityRepository.Get(institutionAvailabilityDto.Id))
                .ReturnsAsync(new InstitutionAvailability()); // Provide a mock instance here if needed

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(validationResult.IsValid);
            _mockUnitOfWork.Verify(uow => uow.InstitutionAvailabilityRepository.Update(It.IsAny<InstitutionAvailability>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ReturnsFailureResult()
        {
            // Arrange
            var institutionAvailabilityDto = new UpdateInstitutionAvailabilityDto
            {
                // Set properties with invalid values to trigger validation errors
            };
            var command = new UpdateInstitutionAvailabilityCommand
            {
                UpdateInstitutionAvailabilityDto = institutionAvailabilityDto
            };
            var validationResult = new UpdateInstitutionAvailabillityDtoValidator().TestValidate(institutionAvailabilityDto);
            _mockUnitOfWork.Setup(uow => uow.InstitutionAvailabilityRepository.Get(It.IsAny<Guid>()))
                .ReturnsAsync((InstitutionAvailability)null); // Return null to simulate not found

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            _mockUnitOfWork.Verify(uow => uow.InstitutionAvailabilityRepository.Update(It.IsAny<InstitutionAvailability>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Never);
        }
    }
}
