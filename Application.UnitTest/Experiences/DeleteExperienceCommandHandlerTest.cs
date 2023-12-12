using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.Persistence;
using Application.Features.Experiences.CQRS.Commands;
using Application.Features.Experiences.CQRS.Handlers;
using AutoMapper;
using Domain;
using Moq;
using Xunit;

namespace Application.UnitTest.Experiences
{
    public class DeleteExperienceCommandHandlerTests
    {
        private readonly DeleteExperienceCommandHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;


        public DeleteExperienceCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new DeleteExperienceCommandHandler(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ExistingExperienceId_ReturnsSuccessResultWithExperienceId()
        {
            // Arrange
            var experienceId = Guid.NewGuid();
            var command = new DeleteExperienceCommand { Id = experienceId };
            var experience = new Experience { Id = experienceId };

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.ExperienceRepository.Get(experienceId))
                .ReturnsAsync(experience);

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.ExperienceRepository.Delete(experience))
                .Verifiable();

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.Save())
                .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(experienceId, result.Value);

            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ExperienceRepository.Get(experienceId), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ExperienceRepository.Delete(experience), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.Save(), Times.Once);
        }

        [Fact]
        public async Task Handle_NonexistentExperienceId_ReturnsNullResult()
        {
            // Arrange
            var experienceId = Guid.NewGuid();
            var command = new DeleteExperienceCommand { Id = experienceId };

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.ExperienceRepository.Get(experienceId))
                .ReturnsAsync((Experience)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);

            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ExperienceRepository.Get(experienceId), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ExperienceRepository.Delete(It.IsAny<Experience>()), Times.Never);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.Save(), Times.Never);
        }

        [Fact]
        public async Task Handle_DeleteFailed_ReturnsFailureResult()
        {
            // Arrange
            var experienceId = Guid.NewGuid();
            var command = new DeleteExperienceCommand { Id = experienceId };
            var experience = new Experience { Id = experienceId };

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.ExperienceRepository.Get(experienceId))
                .ReturnsAsync(experience);

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.ExperienceRepository.Delete(experience))
                .Verifiable();

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.Save())
                .ReturnsAsync(0);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Delete Failed", result.Error);

            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ExperienceRepository.Get(experienceId), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ExperienceRepository.Delete(experience), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.Save(), Times.Once);
        }
    }
}
