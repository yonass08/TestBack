using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.Persistence;
using Application.Features.Services.CQRS.Handlers;
using Application.Features.Services.CQRS.Queries;
using Application.Features.Services.DTOs;
using Application.Responses;
using AutoMapper;
using Domain;
using Moq;
using Xunit;

namespace Application.UnitTest.Services
{
    public class GetServiceByNameQueryHandlerTests
    {
        private readonly GetServiceByNameQueryHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;

        public GetServiceByNameQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetServiceByNameQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidServiceName_ReturnsSuccessResultWithServiceDTO()
        {
            // Arrange
            var query = new GetServiceByNameQuery { ServiceName = "Service 1" };

            var service = new Service { Id = Guid.NewGuid(), ServiceName = "Service 1", ServiceDescription = "Description 1" };

            var serviceDTO = new ServiceDto { Id = Guid.NewGuid(), ServiceName = "Service 1", ServiceDescription = "Description 1" };

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.ServiceRepository.GetServiceByName(query.ServiceName))
                .ReturnsAsync(service);

            _mapperMock
                .Setup(mapper => mapper.Map<ServiceDto>(service))
                .Returns(serviceDTO);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(serviceDTO, result.Value);

            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ServiceRepository.GetServiceByName(query.ServiceName), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<ServiceDto>(service), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidServiceName_ReturnsNullResult()
        {
            // Arrange
            var query = new GetServiceByNameQuery { ServiceName = "Service 1" };

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.ServiceRepository.GetServiceByName(query.ServiceName))
                .ReturnsAsync((Service)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);

            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ServiceRepository.GetServiceByName(query.ServiceName), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<ServiceDto>(It.IsAny<Service>()), Times.Never);
        }
    }
}
