using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.Persistence;
using Application.Features.Experiences.CQRS.Commands;
using Application.Features.Experiences.CQRS.Handlers;
using Application.Features.Experiences.DTOs;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Moq;
using Xunit;

namespace Application.UnitTest.Experiences
{
    public class UpdateExperienceCommandHandlerTests
    {
        private readonly UpdateExperienceCommandHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;

        public UpdateExperienceCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateExperienceCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidExperienceDto_ReturnsSuccessResult()
        {
            // Arrange
            var command = new UpdateExperienceCommand
            {
                ExperienceDto = new UpdateExperienceDto
                {
                    Id = Guid.NewGuid(),
                    Position = "position 1",
                    Description = "Description 1",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    DoctorId = Guid.NewGuid(),
                    InstitutionId = Guid.NewGuid()
                }
            };

            var validationResult = new FluentValidation.Results.ValidationResult();

            var validatorMock = new Mock<IValidator<UpdateExperienceDto>>();
            validatorMock
                .Setup(validator => validator.ValidateAsync(command.ExperienceDto, CancellationToken.None))
                .ReturnsAsync(validationResult);

            var experience = new Experience { Id = Guid.NewGuid() };

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.ExperienceRepository.Get(command.ExperienceDto.Id))
                .ReturnsAsync(experience);

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.ExperienceRepository.Update(experience))
                .Verifiable();

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.Save())
                .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(Unit.Value, result.Value);

            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ExperienceRepository.Get(command.ExperienceDto.Id), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ExperienceRepository.Update(experience), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.Save(), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidExperienceDto_ReturnsFailureResult()
        {
            // Arrange
            var command = new UpdateExperienceCommand
            {
                ExperienceDto = new UpdateExperienceDto
                {
                    Id = Guid.NewGuid(),
                    Position = "",
                    Description = "Description 1",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    DoctorId = Guid.NewGuid(),
                    InstitutionId = Guid.NewGuid()
                }
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Position", "Position is required."));

            var validatorMock = new Mock<IValidator<UpdateExperienceDto>>();
            validatorMock
                .Setup(validator => validator.ValidateAsync(command.ExperienceDto, CancellationToken.None))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Position is required.", result.Error);

            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ExperienceRepository.Get(It.IsAny<Guid>()), Times.Never);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ExperienceRepository.Update(It.IsAny<Experience>()), Times.Never);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.Save(), Times.Never);
        }

        [Fact]
        public async Task Handle_NonexistentExperienceId_ReturnsNullResult()
        {
            // Arrange
            var command = new UpdateExperienceCommand
            {
                ExperienceDto = new UpdateExperienceDto
                {
                    Id = Guid.NewGuid(),
                    Position = "position 1",
                    Description = "Description 1",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    DoctorId = Guid.NewGuid(),
                    InstitutionId = Guid.NewGuid()
                }
            };

            var validationResult = new FluentValidation.Results.ValidationResult();

            var validatorMock = new Mock<IValidator<UpdateExperienceDto>>();
            validatorMock
                .Setup(validator => validator.ValidateAsync(command.ExperienceDto, CancellationToken.None))
                .ReturnsAsync(validationResult);

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.ExperienceRepository.Get(command.ExperienceDto.Id))
                .ReturnsAsync((Experience)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);

            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ExperienceRepository.Get(command.ExperienceDto.Id), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ExperienceRepository.Update(It.IsAny<Experience>()), Times.Never);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.Save(), Times.Never);
        }

        
    }
}
