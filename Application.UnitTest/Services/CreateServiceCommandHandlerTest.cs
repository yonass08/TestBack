using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.Persistence;
using Application.Features.Services.CQRS.Commands;
using Application.Features.Services.CQRS.Handlers;
using Application.Features.Services.DTOs;
using Application.UnitTest.Mocks;
using AutoMapper;
using Domain;
using FluentValidation;
using Moq;
using Shouldly;
using Xunit;

namespace Application.UnitTest.Services
{
    public class CreateServiceCommandHandlerTests
    {
        private readonly CreateServiceCommandHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;

        public CreateServiceCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateServiceCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidServiceDto_ReturnsSuccessResultWithServiceId()
        {
            // Arrange
            var command = new CreateServiceCommand
            {
                ServiceDto = new CreateServiceDto
                {
                    ServiceName = "Test Service",
                    ServiceDescription = "Test Description"
                }
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            var mapperResult = new Service { Id = Guid.NewGuid() };

            var validatorMock = new Mock<IValidator<CreateServiceDto>>();
            validatorMock
                .Setup(validator => validator.ValidateAsync(command.ServiceDto, CancellationToken.None))
                .ReturnsAsync(validationResult);

            _mapperMock
                .Setup(mapper => mapper.Map<Service>(command.ServiceDto))
                .Returns(mapperResult);

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.ServiceRepository.Add(mapperResult))
                .Verifiable();

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.Save())
                .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(mapperResult.Id, result.Value);

            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ServiceRepository.Add(mapperResult), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.Save(), Times.Once);
        }         


        [Fact]
        public async Task Handle_InvalidServiceDto_ReturnsFailureResult()
        {
            // Arrange
            var command = new CreateServiceCommand
            {
                ServiceDto = new CreateServiceDto
                {
                    ServiceName = null, // Invalid service name
                    ServiceDescription = "Test Description"
                }
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("ServiceName", "Service name is required"));

            var validatorMock = new Mock<IValidator<CreateServiceDto>>();
            validatorMock
                .Setup(validator => validator.ValidateAsync(command.ServiceDto, CancellationToken.None))
                .ReturnsAsync(validationResult);

            _mapperMock
                .Setup(mapper => mapper.Map<Service>(command.ServiceDto))
                .Throws(new InvalidOperationException("Mapping should not be invoked"));

            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.ServiceRepository.Add(It.IsAny<Service>()))
                .Throws(new InvalidOperationException("Add should not be invoked"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldNotBeNull();

            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ServiceRepository.Add(It.IsAny<Service>()), Times.Never);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.Save(), Times.Never);
        }

            
        [Fact]
        public async Task Handle_DuplicateServiceName_ReturnsFailureResult()
        {
            // Arrange
            var command = new CreateServiceCommand
            {
                ServiceDto = new CreateServiceDto
                {
                    ServiceName = "Service 1",
                    ServiceDescription = "New Service Description"
                }
            };

            var serviceRepositoryMock = MockServiceRepository.GetServiceRepository();

            _unitOfWorkMock.Setup(uow => uow.ServiceRepository).Returns(serviceRepositoryMock.Object);

            // Adjust the behavior of GetServiceByName to return a service with the same name
            serviceRepositoryMock.Setup(repo => repo.GetServiceByName("Service 1"))
                .ReturnsAsync(new Service
                {
                    Id = Guid.NewGuid(),
                    ServiceName = "Service 1",
                    ServiceDescription = "Existing Service Description"
                });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldNotBeNull();

            serviceRepositoryMock.Verify(repo => repo.Add(It.IsAny<Service>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.Save(), Times.Never);
        }

    }
}

