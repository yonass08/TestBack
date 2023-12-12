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
using Application.Features.DoctorAvailabilities.CQRS.Commands;
using Application.Features.DoctorAvailabilities.CQRS.Handlers;
using Application.Features.DoctorAvailabilities.DTOs;
using Application.Features.DoctorAvailabilities.DTOs.Validators;
using Application.Responses;
using AutoMapper;
using Domain;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace Application.UnitTest.Features.DoctorAvailabilities.CQRS.Handlers
{
    public class UpdateDoctorAvailabilityCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateDoctorAvailabilityCommandHandler _handler;

        public UpdateDoctorAvailabilityCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new UpdateDoctorAvailabilityCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsSuccessResult()
        {
            // Arrange
            var doctorAvailabilityDto = new UpdateDoctorAvailabilityDto
            {
                Id = Guid.NewGuid(),
                  Day = DayOfWeek.Friday,
                    StartTime = "2:00 AM",
                    EndTime = "3:00 PM",
                    DoctorId = Guid.NewGuid(),
                    InstitutionId = Guid.NewGuid(),
                    SpecialityId = Guid.NewGuid(),

            };
            var command = new UpdateDoctorAvailabilityCommand
            {
                UpdateDoctorAvailabilityDto = doctorAvailabilityDto
            };
            var validationResult = new UpdateDoctorAvailabillityDtoValidator().Validate(doctorAvailabilityDto);
            _mockUnitOfWork.Setup(uow => uow.DoctorAvailabilityRepository.Get(doctorAvailabilityDto.Id))
                .ReturnsAsync(new DoctorAvailability()); // Provide a mock instance here if needed

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(validationResult.IsValid);
            _mockUnitOfWork.Verify(uow => uow.DoctorAvailabilityRepository.Update(It.IsAny<DoctorAvailability>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ReturnsFailureResult()
        {
            // Arrange
            var doctorAvailabilityDto = new UpdateDoctorAvailabilityDto
            {
                // Set properties with invalid values to trigger validation errors
            };
            var command = new UpdateDoctorAvailabilityCommand
            {
                UpdateDoctorAvailabilityDto = doctorAvailabilityDto
            };
            var validationResult = new UpdateDoctorAvailabillityDtoValidator().TestValidate(doctorAvailabilityDto);
            _mockUnitOfWork.Setup(uow => uow.DoctorAvailabilityRepository.Get(It.IsAny<Guid>()))
                .ReturnsAsync((DoctorAvailability)null); // Return null to simulate not found

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            _mockUnitOfWork.Verify(uow => uow.DoctorAvailabilityRepository.Update(It.IsAny<DoctorAvailability>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Never);
        }
    }
}
