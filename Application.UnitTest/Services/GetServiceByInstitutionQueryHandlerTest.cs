using System.Collections.Generic;
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
    public class GetServiceByInstitutionQueryHandlerTests
    {
        private readonly GetServiceByInstitutionQueryHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;

        public GetServiceByInstitutionQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetServiceByInstitutionQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidInstitutionId_ReturnsSuccessResultWithServiceDTOs()
        {
            // Arrange
            var query = new GetServiceByInstitutionQuery { InstitutionId = Guid.NewGuid() };

            var services = new List<Service>
            {
                new Service { Id = Guid.NewGuid(), ServiceName = "Service 1", ServiceDescription = "Description 1" },
                new Service { Id = Guid.NewGuid(), ServiceName = "Service 2", ServiceDescription = "Description 2" }
            };

            var serviceDTOs = new List<ServiceDto>
            {
                new ServiceDto { Id = Guid.NewGuid(), ServiceName = "Service 1", ServiceDescription = "Description 1" },
                new ServiceDto { Id = Guid.NewGuid(), ServiceName = "Service 2", ServiceDescription = "Description 2" }
            };

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.ServiceRepository.GetServicesByInstitutionId(query.InstitutionId))
                .ReturnsAsync(services);

            _mapperMock
                .Setup(mapper => mapper.Map<List<ServiceDto>>(services))
                .Returns(serviceDTOs);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(serviceDTOs, result.Value);

            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ServiceRepository.GetServicesByInstitutionId(query.InstitutionId), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<List<ServiceDto>>(services), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidInstitutionId_ReturnsNullResult()
        {
            // Arrange
            var query = new GetServiceByInstitutionQuery { InstitutionId = Guid.NewGuid() };

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.ServiceRepository.GetServicesByInstitutionId(query.InstitutionId))
                .ReturnsAsync((List<Service>)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);

            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ServiceRepository.GetServicesByInstitutionId(query.InstitutionId), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<List<ServiceDto>>(It.IsAny<List<Service>>()), Times.Never);
        }
    }
}
