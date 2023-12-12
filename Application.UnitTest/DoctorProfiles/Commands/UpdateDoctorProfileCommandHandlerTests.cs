using Moq;
using AutoMapper;
using Application.Features.DoctorProfiles.CQRS.Commands;
using Application.Features.DoctorProfiles.DTOs;
using static Domain.DoctorProfile;
using Application.Features.DoctorProfiles.CQRS.Handlers;
using Application.Profiles;
using Application.UnitTest.Mocks;
using Application.Contracts.Persistence;
using Shouldly;
using Domain;
using Application.Interfaces;
using Application.Responses;
using MediatR;
using Application.Photos;
using Microsoft.AspNetCore.Http;

namespace Application.UnitTest.DoctorProfiles.Commands;

public class UpdateDoctorProfileCommandHandlerTests
{
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private IMapper _mapper;
    private UpdateDoctorProfileCommandHandler _handler;
    private Mock<IPhotoAccessor> _mockPhotoAccessor;

    public UpdateDoctorProfileCommandHandlerTests()
    {
        _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
        _mockPhotoAccessor = new Mock<IPhotoAccessor>();

        var mapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile<MappingProfile>();
        });
        _mapper = mapperConfig.CreateMapper();
        _handler = new UpdateDoctorProfileCommandHandler(_mockUnitOfWork.Object, _mapper, _mockPhotoAccessor.Object);
    }

    [Fact]
    public async Task Handle_WithValidUpdate_ShouldReturnSuccessResult()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("doctor.jpg");
        fileMock.Setup(f => f.Length).Returns(100); // Set the file length as needed
        var updateDto = new UpdateDoctorProfileDto
        {
            Id = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3301"),
            FullName = "Abeebe",
            About = "Anjncaan",
            Email = "anacnanc@gmai.com",
            CareerStartTime = DateTime.Parse("2022-06-10T09:15:26.533993Z"),
            Gender = GenderType.Male.ToString().ToLower(),
            DoctorPhoto = fileMock.Object
        };

        var command = new UpdateDoctorProfileCommand
        {
            updateDoctorProfileDto = updateDto
        };

        _mockPhotoAccessor.Setup(pa => pa.AddPhoto(It.IsAny<IFormFile>())).ReturnsAsync(new PhotoUploadResult
        {
            PublicId = "photo1",
            Url = "http://example.com/photo1.jpg"
        });
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.ShouldBeOfType<Result<Unit>>();
        
    }

    [Fact]
    public async Task Handle_WithInvalidData_ShouldReturnFailureResultWithErrors()
    {
        // Arrange
        var updateDto = new UpdateDoctorProfileDto
        {
            Id = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3301"),
            FullName = null,
            About = "hencaman",
            Email = "anacnanc@gmai.com",
            CareerStartTime = DateTime.Parse("2022-06-10T09:15:26.533993Z"),
            Gender = GenderType.Male.ToString().ToLower(),

        };

        var command = new UpdateDoctorProfileCommand
        {
            updateDoctorProfileDto = updateDto
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldBeOfType<Result<Unit>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldContain("Full Name is required");
        result.Error.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task Handle_WithNonExistentDoctorProfile_ShouldReturnFailureResultWithNotFoundMessage()
    {
        // Arrange
        var updateDto = new UpdateDoctorProfileDto
        {
            Id = Guid.Parse("518b301a-30da-4a1e-97e0-e7d58d18ba75"),
            FullName = "Gete",
            About = "hencaman",
            Email = "anacnanc@gmai.com",
            CareerStartTime = DateTime.Parse("2022-06-10T09:15:26.533993Z"),
            Gender = GenderType.Male.ToString().ToLower(), // Provide a non-existent ID here
        };

        var command = new UpdateDoctorProfileCommand
        {
            updateDoctorProfileDto = updateDto
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldBeOfType<Result<Unit>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldContain("The doctorProfile with 518b301a-30da-4a1e-97e0-e7d58d18ba75 not found");
    }

    [Fact]
    public async Task Handle_WithFailedPhotoUpload_ShouldReturnFailureResultWithErrorMessage()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("doctor.jpg");
        fileMock.Setup(f => f.Length).Returns(100); // Set the file length as needed
        var updateDto = new UpdateDoctorProfileDto
        {
            Id = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3301"),
            FullName = "Abeebe",
            About = "Anjncaan",
            Email = "anacnanc@gmai.com",
            CareerStartTime = DateTime.Parse("2022-06-10T09:15:26.533993Z"),
            Gender = GenderType.Male.ToString().ToLower(),
            DoctorPhoto = fileMock.Object
        };

        var command = new UpdateDoctorProfileCommand
        {
            updateDoctorProfileDto = updateDto
        };


        _mockPhotoAccessor.Setup(pa => pa.AddPhoto(It.IsAny<IFormFile>()))
            .ReturnsAsync((PhotoUploadResult)null); // Simulate failed photo upload

        // Mock the necessary dependencies to return the expected results

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldBeOfType<Result<Unit>>();
        result.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task Handle_WithSuccessfulPhotoUpload_ShouldUpdateDoctorProfileWithPhotoInfo()
    { // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("doctor.jpg");
        fileMock.Setup(f => f.Length).Returns(100); // Set the file length as needed
        var updateDto = new UpdateDoctorProfileDto
        {
            Id = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3301"),
            FullName = "Abeebe",
            About = "Anjncaan",
            Email = "anacnanc@gmai.com",
            CareerStartTime = DateTime.Parse("2022-06-10T09:15:26.533993Z"),
            Gender = GenderType.Male.ToString().ToLower(),
            DoctorPhoto = fileMock.Object
        };

        var command = new UpdateDoctorProfileCommand
        {
            updateDoctorProfileDto = updateDto
        };


        var photoUploadResult = new PhotoUploadResult
        {
            PublicId = "photo123",
            Url = "https://example.com/photo123.jpg"
        };

        _mockPhotoAccessor.Setup(pa => pa.AddPhoto(It.IsAny<IFormFile>()))
            .ReturnsAsync(photoUploadResult);

        // Mock the necessary dependencies to return the expected results

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldBeOfType<Result<Unit>>();
        result.IsSuccess.ShouldBeTrue();
       
    }

    // [Fact]
    // public async Task Handle_WithInvalidPhotoExtension_ShouldReturnValidationFailure()
    // {
    //     // Arrange
    //     var fileMock = new Mock<IFormFile>();
    //     fileMock.Setup(f => f.FileName).Returns("doctor.doc");
    //     fileMock.Setup(f => f.Length).Returns(100); // Set the file length as needed
    //     var updateDto = new UpdateDoctorProfileDto
    //     {
    //         Id = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3301"),
    //         FullName = "Abeebe",
    //         About = "Anjncaan",
    //         Email = "anacnanc@gmai.com",
    //         CareerStartTime = DateTime.Parse("2022-06-10T09:15:26.533993Z"),
    //         Gender = GenderType.Male.ToString().ToLower(),
    //         DoctorPhoto = fileMock.Object
    //     };

    //     var command = new UpdateDoctorProfileCommand
    //     {
    //         updateDoctorProfileDto = updateDto


    //     };
    //     _mockPhotoAccessor.Setup(pa => pa.AddPhoto(It.IsAny<IFormFile>())).ReturnsAsync(new PhotoUploadResult
    //     {
    //         PublicId = "photo123",
    //         Url = "https://example.com/photo123.jpg"
    //     });


    //     // Act
    //     var result = await _handler.Handle(command, CancellationToken.None);

    //     // Assert
    //     result.ShouldNotBeNull();
    //     result.IsSuccess.ShouldBeFalse();
    //     result.Error.ShouldBe("Doctor Photo must have a valid file extension");

    //     _mockUnitOfWork.Verify(uow => uow.DoctorProfileRepository.Update(It.IsAny<DoctorProfile>()), Times.Never);
    //     _mockUnitOfWork.Verify(uow => uow.Save(), Times.Never);
    // }
}

