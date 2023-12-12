using NUnit.Framework;
using Moq;
using FluentValidation.Results;
using System.Linq;
using Application.Interfaces;
using Application.Contracts.Persistence;
using AutoMapper;
using Application.Profiles;
using Application.Features.DoctorProfiles.CQRS.Handlers;
using Application.Features.DoctorProfiles.CQRS.Commands;
using Application.Features.DoctorProfiles.DTOs;
using Microsoft.AspNetCore.Http;
using Domain;
using Application.Photos;
using Shouldly;
using Application.Responses;
using FluentValidation;
using Application.UnitTest.Mocks;
using System.Text;
namespace Application.UnitTest.DoctorProfiles.Commands;
public class CreateDoctorProfileCommandHandlerTests
{
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private IMapper _mapper;
    private CreateDoctorProfileCommandHandler _handler;
    private Mock<IPhotoAccessor> _mockPhotoAccessor;


    public CreateDoctorProfileCommandHandlerTests()
    {
        _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
        _mockPhotoAccessor = new Mock<IPhotoAccessor>();

        var mapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile<MappingProfile>();
        });
        _mapper = mapperConfig.CreateMapper();
        _handler = new CreateDoctorProfileCommandHandler(_mockUnitOfWork.Object, _mapper, _mockPhotoAccessor.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateDoctorProfile()
    {


        // Arrange
        var createDoctorProfileDto = new CreateDoctorProfileDto
        {
            FullName = "Dr. John Smith",
            About = "Experienced and skilled doctor",
            Gender = "male",
            Email = "john.smith@example.com",
            CareerStartTime = DateTime.Now,
        };
        var command = new CreateDoctorProfileCommand
        {
            CreateDoctorProfileDto = createDoctorProfileDto
        };

        _mockPhotoAccessor.Setup(pa => pa.AddPhoto(It.IsAny<IFormFile>())).ReturnsAsync(new PhotoUploadResult
        {
            PublicId = "photo1",
            Url = "http://example.com/photo1.jpg"
        });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.ShouldBeOfType<Result<Guid>>();

        _mockUnitOfWork.Verify(uow => uow.DoctorProfileRepository.Add(It.IsAny<DoctorProfile>()), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
        _mockPhotoAccessor.Verify(pa => pa.AddPhoto(It.IsAny<IFormFile>()), Times.AtMostOnce);
    }


    [Fact]
    public async Task Handle_WithInvalidCommand_ShouldReturnValidationFailure()
    {
        // Arrange
        var CreateDoctorProfileDto = new CreateDoctorProfileDto
        {
            FullName = null, // Invalid: Missing required field
            Gender = "male",
            CareerStartTime = DateTime.Now
        };
        var command = new CreateDoctorProfileCommand
        {
            CreateDoctorProfileDto = CreateDoctorProfileDto

        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldContain("Full Name is required");

        _mockUnitOfWork.Verify(uow => uow.DoctorProfileRepository.Add(It.IsAny<DoctorProfile>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.Save(), Times.Never);
        _mockPhotoAccessor.Verify(pa => pa.AddPhoto(It.IsAny<IFormFile>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithFailedPhotoUpload_ShouldReturnFailure()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("doctor.jpg");
        fileMock.Setup(f => f.Length).Returns(100); // Set the file length as needed

        var command = new CreateDoctorProfileCommand
        {
            CreateDoctorProfileDto = new CreateDoctorProfileDto
            {
                FullName = "Dr. John Smith",
                Gender = "male",
                CareerStartTime = DateTime.Now,
                DoctorPhoto = fileMock.Object
            }
        };

        _mockPhotoAccessor.Setup(pa => pa.AddPhoto(It.IsAny<IFormFile>())).ReturnsAsync((PhotoUploadResult?)null); // Failed photo upload

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.ShouldBeOfType<Result<Guid>>();
        result.Error.ShouldBe("photo upload failed");

        _mockUnitOfWork.Verify(uow => uow.DoctorProfileRepository.Add(It.IsAny<DoctorProfile>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.Save(), Times.Never);
        _mockPhotoAccessor.Verify(pa => pa.AddPhoto(It.IsAny<IFormFile>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithServerErrorDuringProfileCreation_ShouldReturnFailure()
    {
        // Arrange
        var command = new CreateDoctorProfileCommand
        {
            CreateDoctorProfileDto = new CreateDoctorProfileDto
            {
                FullName = "faile",
                Gender = "male",
                CareerStartTime = DateTime.Now,
            }
        };

        var mockDoctor = new Mock<IUnitOfWork>();
        mockDoctor.Setup(uow => uow.DoctorProfileRepository.Add(It.IsAny<DoctorProfile>()))
               .ReturnsAsync((DoctorProfile doctor) =>
               {
                   doctor.Id = new Guid();
                   return doctor;

               });
        mockDoctor.Setup(uow => uow.Save()).ReturnsAsync(0); // Server error during save

        var serverHandler = new CreateDoctorProfileCommandHandler(mockDoctor.Object, _mapper, _mockPhotoAccessor.Object);
        _mockPhotoAccessor.Setup(pa => pa.AddPhoto(It.IsAny<IFormFile>())).ReturnsAsync(new PhotoUploadResult
        {
            PublicId = "photo1",
            Url = "http://example.com/photo1.jpg"
        });

        // Act
        var result = await serverHandler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("Server error");

        mockDoctor.Verify(uow => uow.DoctorProfileRepository.Add(It.IsAny<DoctorProfile>()), Times.Once);
        mockDoctor.Verify(uow => uow.Save(), Times.Once);
        _mockPhotoAccessor.Verify(pa => pa.AddPhoto(It.IsAny<IFormFile>()), Times.AtMostOnce);
    }

    [Fact]
    public async Task Handle_WithoutPhoto_ShouldCreateDoctorProfile()
    {
        // Arrange
        var command = new CreateDoctorProfileCommand
        {
            CreateDoctorProfileDto = new CreateDoctorProfileDto
            {
                FullName = "Dr. John Smith",
                Gender = "male",
                CareerStartTime = DateTime.Now,
                DoctorPhoto = null // No photo provided
            }
        };


        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.ShouldBeOfType<Result<Guid>>();

        _mockUnitOfWork.Verify(uow => uow.DoctorProfileRepository.Add(It.IsAny<DoctorProfile>()), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
        _mockPhotoAccessor.Verify(pa => pa.AddPhoto(It.IsAny<IFormFile>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithInvalidPhotoExtension_ShouldReturnValidationFailure()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("doctor.doc");
        fileMock.Setup(f => f.Length).Returns(100); // Set the file length as needed

        var command = new CreateDoctorProfileCommand
        {
            CreateDoctorProfileDto = new CreateDoctorProfileDto
            {
                FullName = "Dr. John Smith",
                Gender = "male",
                CareerStartTime = DateTime.Now,
                DoctorPhoto = fileMock.Object
            }
        };



        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("photo upload failed");

        _mockUnitOfWork.Verify(uow => uow.DoctorProfileRepository.Add(It.IsAny<DoctorProfile>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.Save(), Times.Never);
    }

    [Fact]
    public async Task Handle_WithInvalidGender_ShouldReturnValidationFailure()
    {
        // Arrange
        var command = new CreateDoctorProfileCommand
        {
            CreateDoctorProfileDto = new CreateDoctorProfileDto
            {
                FullName = "Dr. John Smith",
                Gender = "unknown", // Invalid gender value
                CareerStartTime = DateTime.Now,
            }
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldContain("Gender must be 'male' or 'female'");

        _mockUnitOfWork.Verify(uow => uow.DoctorProfileRepository.Add(It.IsAny<DoctorProfile>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.Save(), Times.Never);
        _mockPhotoAccessor.Verify(pa => pa.AddPhoto(It.IsAny<IFormFile>()), Times.Never);
    }
}
