using Xunit;
using Moq;
using Application.Features.InstitutionAvailabilities.CQRS.Commands;
using Application.Features.InstitutionAvailabilities.CQRS.Handlers;
using Application.Features.InstitutionAvailabilities.DTOs;
using Application.Responses;
using Application.Contracts.Persistence;
using AutoMapper;
using System.Threading;
using Domain;
using System;
using FluentValidation.TestHelper;

namespace Application.UnitTest.InstitutionAvailabilities.CQRS.Handlers
{
    public class CreateInstitutionAvailabilityCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;

        public CreateInstitutionAvailabilityCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldReturnSuccessResultWithGuid()
        {
            // Arrange
            var command = new CreateInstitutionAvailabilityCommand
            {
                CreateInstitutionAvailabilityDto = new CreateInstitutionAvailabilityDto
                {
                    StartDay = "Monday",
                    EndDay = "Sunday",
                    Opening = "2:00AM",
                    Closing = "4:00PM",
                    TwentyFourHours = true,
                    InstitutionId = Guid.NewGuid()
                }
            };

            var handler = new CreateInstitutionAvailabilityCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);

            _mockMapper.Setup(m => m.Map<InstitutionAvailability>(command.CreateInstitutionAvailabilityDto))
                .Returns(new InstitutionAvailability { Id = Guid.NewGuid() });

            _mockUnitOfWork.Setup(uow => uow.InstitutionAvailabilityRepository.Add(It.IsAny<InstitutionAvailability>()))
                .ReturnsAsync((InstitutionAvailability institutionAvailability) => institutionAvailability);

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
    var command = new CreateInstitutionAvailabilityCommand
    {
        CreateInstitutionAvailabilityDto = new CreateInstitutionAvailabilityDto
        {
            // Missing required properties or invalid data
        }
    };

    var handler = new CreateInstitutionAvailabilityCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    
    Assert.False(result.IsSuccess);
    Assert.NotNull(result.Error);
    Assert.NotEmpty(result.Error);
}

    }
}
