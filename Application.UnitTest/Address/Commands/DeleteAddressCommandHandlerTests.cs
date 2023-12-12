using AutoMapper;
using Moq;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Application.Contracts.Persistence;
using Application.Features.Addresses.CQRS.Commands;
using Application.Features.Addresses.CQRS.Handlers;
using Application.Responses;
using Domain;

namespace Application.UnitTest.Addresses.Commands
{
    public class DeleteAddressCommandHandlerTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IAddressRepository> _mockAddressRepository;
        private readonly DeleteAddressCommandHandler _handler;

        public DeleteAddressCommandHandlerTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockAddressRepository = new Mock<IAddressRepository>();
            _handler = new DeleteAddressCommandHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task DeleteAddress_ValidId_ShouldReturnSuccessResult()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var deleteCommand = new DeleteAddressCommand { Id = addressId };
            var addressToDelete = new Address { Id = addressId };

            _mockAddressRepository.Setup(r => r.Get(addressId)).ReturnsAsync(addressToDelete);
            _mockUnitOfWork.Setup(uow => uow.AddressRepository).Returns(_mockAddressRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.Save()).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(deleteCommand, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<Result<Guid>>();
            result.IsSuccess.ShouldBeTrue();
            result.Value.ShouldBe(addressId);
        }

        [Fact]
        public async Task DeleteAddress_InvalidId_ShouldReturnFailureResult()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var deleteCommand = new DeleteAddressCommand { Id = addressId };

            _mockAddressRepository.Setup(r => r.Get(addressId)).ReturnsAsync((Address)null);
            _mockUnitOfWork.Setup(uow => uow.AddressRepository).Returns(_mockAddressRepository.Object);

            // Act
            var result = await _handler.Handle(deleteCommand, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<Result<Guid>>();
            result.IsSuccess.ShouldBeFalse();
            result.Value.ShouldBe(Guid.Empty);
            result.Error.ShouldNotBeEmpty();
        }
    }
}
