using Application.Contracts.Persistence;
using Application.Features.Educations.CQRS;
using Application.Features.Educations.CQRS.Handlers;
using Application.Features.Educations.DTOs;
using Application.Interfaces;
using Application.Photos;
using Application.Profiles;
using Application.UnitTest.Mocks;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;

namespace Application.UnitTest.EducationTest.EducationCommandsTest;

public class CreateEducationCommandHandlerTest
{
    private Mock<IMapper> _mapper { get; set; }
    private Mock<IUnitOfWork> _mockUnitOfWork { get; set; }

    private readonly Mock<IPhotoAccessor> _mockPhotoAccessor;

    private readonly CreateEducationDto createEducationDto;
    private CreateEducationCommandHandler _handler { get; set; }
    public CreateEducationCommandHandlerTest()
    {
        _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();

        _mapper = new Mock<IMapper>();

        _mockPhotoAccessor = new Mock<IPhotoAccessor>();

         createEducationDto = new CreateEducationDto()
        {
            Id = Guid.NewGuid(),
            EducationInstitution = "Oxford University",
            StartYear = DateTime.Now,
            GraduationYear = DateTime.Today,
            FieldOfStudy = "Oncology",
            Degree = "Bachelors",
            DoctorId = Guid.NewGuid(),
            // EducationInstitutionLogoId = "Oxford Campus",
        };

        _handler = new CreateEducationCommandHandler(_mockUnitOfWork.Object, _mapper.Object, _mockPhotoAccessor.Object);
    }

    [Fact]
    public async Task CreateEducationValid()
    {

        var photo = new PhotoUploadResult { PublicId = "1000", Url = "photo-public-id" };
        _mockPhotoAccessor.Setup(pa => pa.AddPhoto(It.IsAny<IFormFile>())).ReturnsAsync(photo);


        var education = new Education { Id = Guid.NewGuid() };
        _mapper.Setup(x => x.Map<Education>(createEducationDto)).Returns(education);

        var result = await _handler.Handle(new CreateEducationCommand{createEducationDto = createEducationDto}, CancellationToken.None);
            
        result.IsSuccess.ShouldBeTrue();
        _mockUnitOfWork.Verify(x => x.Save(), Times.Exactly(1));
        var repo = await _mockUnitOfWork.Object.EducationRepository.GetAll();
        repo.Count.ShouldBe(3);
    }

    [Fact]
    public async Task CreateEducationInvalid()
    {
    
        CreateEducationDto createInvalid = new CreateEducationDto()
        {
            Id = Guid.NewGuid(),
            EducationInstitution = null,
            StartYear = DateTime.Now,
            GraduationYear = DateTime.Today,
        };

        var result = await _handler.Handle(new CreateEducationCommand() { createEducationDto = createInvalid }, CancellationToken.None);

        result.Value.ShouldBe(null);
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldNotBeEmpty();
        _mockUnitOfWork.Verify(x => x.Save(), Times.Never());
    }
}

