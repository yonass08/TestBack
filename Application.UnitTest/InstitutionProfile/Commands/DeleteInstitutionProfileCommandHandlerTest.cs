using AutoMapper;
using Moq;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Application.Contracts.Persistence;
using Application.Features.InstitutionProfiles.CQRS.Commands;
using Application.Features.InstitutionProfiles.CQRS.Handlers;
using Application.Responses;
using Domain;

namespace Application.UnitTest.InstitutionProfiles.Commands
{
    public class DeleteInstitutionProfileCommandHandlerTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IInstitutionProfileRepository> _mockInstitutionProfileRepository;
        private readonly DeleteInstitutionProfileCommandHandler _handler;

        public DeleteInstitutionProfileCommandHandlerTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockInstitutionProfileRepository = new Mock<IInstitutionProfileRepository>();
            _handler = new DeleteInstitutionProfileCommandHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task DeleteInstitutionProfile_ValidId_ShouldReturnSuccessResult()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var deleteCommand = new DeleteInstitutionProfileCommand { Id = addressId };
            var addressToDelete = new InstitutionProfile { Id = addressId };

            _mockInstitutionProfileRepository.Setup(r => r.Get(addressId)).ReturnsAsync(addressToDelete);
            _mockUnitOfWork.Setup(uow => uow.InstitutionProfileRepository).Returns(_mockInstitutionProfileRepository.Object);
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
        public async Task DeleteInstitutionProfile_InvalidId_ShouldReturnFailureResult()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var deleteCommand = new DeleteInstitutionProfileCommand { Id = addressId };

            _mockInstitutionProfileRepository.Setup(r => r.Get(addressId)).ReturnsAsync((InstitutionProfile)null);
            _mockUnitOfWork.Setup(uow => uow.InstitutionProfileRepository).Returns(_mockInstitutionProfileRepository.Object);

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
