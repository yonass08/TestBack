using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.Persistence;
using Application.Features.Services.CQRS.Commands;
using Application.Features.Services.CQRS.Handlers;
using Application.Responses;
using AutoMapper;
using Domain;
using Moq;
using Xunit;

namespace Application.UnitTest.Services
{
    public class DeleteServiceCommandHandlerTests
    {
        private readonly DeleteServiceCommandHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;


        public DeleteServiceCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new DeleteServiceCommandHandler(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ExistingServiceId_ReturnsSuccessResultWithServiceId()
        {
            // Arrange
            var serviceId = Guid.NewGuid();
            var command = new DeleteServiceCommand { Id = serviceId };
            var service = new Service { Id = serviceId };

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.ServiceRepository.Get(serviceId))
                .ReturnsAsync(service);

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.ServiceRepository.Delete(service))
                .Verifiable();

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.Save())
                .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(serviceId, result.Value);

            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ServiceRepository.Get(serviceId), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ServiceRepository.Delete(service), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.Save(), Times.Once);
        }

        [Fact]
        public async Task Handle_NonexistentServiceId_ReturnsNullResult()
        {
            // Arrange
            var serviceId = Guid.NewGuid();
            var command = new DeleteServiceCommand { Id = serviceId };

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.ServiceRepository.Get(serviceId))
                .ReturnsAsync((Service)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);

            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ServiceRepository.Get(serviceId), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ServiceRepository.Delete(It.IsAny<Service>()), Times.Never);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.Save(), Times.Never);
        }

        [Fact]
        public async Task Handle_DeleteFailed_ReturnsFailureResult()
        {
            // Arrange
            var serviceId = Guid.NewGuid();
            var command = new DeleteServiceCommand { Id = serviceId };
            var service = new Service { Id = serviceId };

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.ServiceRepository.Get(serviceId))
                .ReturnsAsync(service);

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.ServiceRepository.Delete(service))
                .Verifiable();

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.Save())
                .ReturnsAsync(0);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Delete Failed", result.Error);

            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ServiceRepository.Get(serviceId), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ServiceRepository.Delete(service), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.Save(), Times.Once);
        }
    }
}
