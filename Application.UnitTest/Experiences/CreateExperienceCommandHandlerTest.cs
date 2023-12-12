using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.Persistence;
using Application.Features.Experiences.CQRS.Commands;
using Application.Features.Experiences.CQRS.Handlers;
using Application.Features.Experiences.DTOs;
using AutoMapper;
using Domain;
using FluentValidation;
using Moq;
using Xunit;

namespace Application.UnitTest.Experiences
{
    public class CreateExperienceCommandHandlerTests
    {
        private readonly CreateExperienceCommandHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;

        public CreateExperienceCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _handler = new CreateExperienceCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidExperienceDto_ReturnsSuccessResultWithExperienceId()
        {
            // Arrange
            var command = new CreateExperienceCommand
            {
                ExperienceDto = new CreateExperienceDto
                {
                    Position = "position 1",
                    Description = "Description 1",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    DoctorId = Guid.NewGuid(),
                    InstitutionId = Guid.NewGuid()
                }
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            var mapperResult = new Experience { Id = Guid.NewGuid() };

            var validatorMock = new Mock<IValidator<CreateExperienceDto>>();
            validatorMock
                .Setup(validator => validator.ValidateAsync(command.ExperienceDto, CancellationToken.None))
                .ReturnsAsync(validationResult);

            _mapperMock
                .Setup(mapper => mapper.Map<Experience>(command.ExperienceDto))
                .Returns(mapperResult);

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.ExperienceRepository.Add(mapperResult))
                .Verifiable();

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.Save())
                .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(mapperResult.Id, result.Value);

            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ExperienceRepository.Add(mapperResult), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.Save(), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidExperienceDto_ReturnsFailureResultWithError()
        {
            // Arrange
            var command = new CreateExperienceCommand
            {
                ExperienceDto = new CreateExperienceDto
                {
                    Position = "",
                    Description = "Description 1",
                    StartDate = DateTime.MinValue,
                    EndDate = DateTime.MaxValue,
                    DoctorId = Guid.NewGuid(),
                    InstitutionId = Guid.NewGuid()
                }
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Position", "Position is required."));

            var validatorMock = new Mock<IValidator<CreateExperienceDto>>();
            validatorMock
                .Setup(validator => validator.ValidateAsync(command.ExperienceDto, CancellationToken.None))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Position is required.", result.Error);
        }
    }
}
