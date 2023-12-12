using Application.Contracts.Persistence;
using Application.Features.DoctorProfiles.CQRS.Commands;
using Application.Features.DoctorProfiles.CQRS.Handlers;
using Application.Profiles;
using Application.Responses;
using Application.UnitTest.Mocks;
using AutoMapper;
using Domain;
using MediatR;
using Moq;
using Shouldly;

namespace Application.UnitTest.DoctorProfiles.Commands
{

    public class DeleteDoctorProfileCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly IMapper _mapper;
        private DeleteDoctorProfileCommandHandler _handler;

        public DeleteDoctorProfileCommandHandlerTests()
        {
            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();

            _handler = new DeleteDoctorProfileCommandHandler(_mockUnitOfWork.Object, _mapper);
        }

        [Fact]
        public async Task DeleteDoctorProfileWithValidId_ReturnsSuccessResult()
        {
            // Arrange
            var doctorId = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3304");
            var command = new DeleteDoctorProfileCommand { Id = doctorId };
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.ShouldBeOfType<Result<Unit>>();
        }

        [Fact]
        public async Task DeleteDoctorProfileWithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var doctorId = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3314");
            var command = new DeleteDoctorProfileCommand { Id = doctorId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldNotBeNull();
            result.ShouldBeOfType<Result<Unit>>();
        }

        [Fact]
        public async Task DeleteDoctorProfile_ReturnsServerError_WhenSaveFails()
        {
            // Arrange
            var doctorId = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3304");
            var command = new DeleteDoctorProfileCommand { Id = doctorId };
            var existingDoctorProfile = new DoctorProfile { Id = doctorId };
            
            _mockUnitOfWork.Setup(uow => uow.DoctorProfileRepository.GetDoctorProfileDetail(doctorId))
                .ReturnsAsync(existingDoctorProfile);
            _mockUnitOfWork.Setup(uow => uow.Save())
                .ReturnsAsync(0);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldNotBeNull();
            result.Error.ShouldBe("server error");
            result.ShouldBeOfType<Result<Unit>>();
        }
    }

}
