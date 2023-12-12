using Xunit;
using Moq;
using Application.Features.DoctorAvailabilities.CQRS.Commands;
using Application.Features.DoctorAvailabilities.CQRS.Handlers;
using Application.Features.DoctorAvailabilities.DTOs;
using Application.Responses;
using Application.Contracts.Persistence;
using AutoMapper;
using System.Threading;
using Domain;
using System;
using FluentValidation.TestHelper;

namespace Application.UnitTest.DoctorAvailabilities.CQRS.Handlers
{
    public class CreateDoctorAvailabilityCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;

        public CreateDoctorAvailabilityCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldReturnSuccessResultWithGuid()
        {
            // Arrange
            var command = new CreateDoctorAvailabilityCommand
            {
                CreateDoctorAvailabilityDto = new CreateDoctorAvailabilityDto
                {
                    Day = DayOfWeek.Friday,
                    StartTime = "2:00 AM",
                    EndTime = "3:00 PM",
                    DoctorId = Guid.NewGuid(),
                    InstitutionId = Guid.NewGuid(),
                    SpecialityId = Guid.NewGuid(),
                }
            };

            var handler = new CreateDoctorAvailabilityCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);

            _mockMapper.Setup(m => m.Map<DoctorAvailability>(command.CreateDoctorAvailabilityDto))
                .Returns(new DoctorAvailability { Id = Guid.NewGuid() });

            _mockUnitOfWork.Setup(uow => uow.DoctorAvailabilityRepository.Add(It.IsAny<DoctorAvailability>()))
                .ReturnsAsync((DoctorAvailability doctorAvailability) => doctorAvailability);

            _mockUnitOfWork.Setup(uow => uow.Save())
                .ReturnsAsync(1);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.IsType<Guid>(result.Value);
            Assert.Null(result.Error);
        }

        
       [Fact]
public async Task Handle_InvalidRequest_ShouldReturnFailureResultWithError()
{
    // Arrange
    var command = new CreateDoctorAvailabilityCommand
    {
        CreateDoctorAvailabilityDto = new CreateDoctorAvailabilityDto
        {
            // Missing required properties or invalid data
        }
    };

    var handler = new CreateDoctorAvailabilityCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);

    // Act
    var result = await handler.Handle(command, CancellationToken.None);
    

    // Assert
    
    Assert.False(result.IsSuccess);
    Assert.NotNull(result.Error);
    Assert.NotEmpty(result.Error);
}

    }
}
