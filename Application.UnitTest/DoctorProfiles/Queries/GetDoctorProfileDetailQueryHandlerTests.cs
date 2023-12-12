using Moq;
using AutoMapper;
using Application.Contracts.Persistence;
using Application.Features.DoctorProfiles.CQRS.Handlers;
using Application.Profiles;
using Domain;
using Application.Features.DoctorProfiles.CQRS.Queris;
using Shouldly;
using Application.Responses;
using Application.Features.DoctorProfiles.DTOs;
using Application.UnitTest.Mocks;

public class GetDoctorProfileDetailQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly GetDoctorProfileDetailQueryHandler _handler;

    public GetDoctorProfileDetailQueryHandlerTests()
    {
        _mockUow = MockUnitOfWork.GetUnitOfWork();

        var mapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile<MappingProfile>();
        });

        _mapper = mapperConfig.CreateMapper();
        _handler = new GetDoctorProfileDetailQueryHandler(_mockUow.Object, _mapper);
    }

    [Fact]
    public async Task Handle_ExistingDoctorProfile_ReturnsSuccessResult()
    {
        // Arrange
        var doctorProfileId = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3304");
        var expectedDoctorProfile = new DoctorProfileDetailDto
        {
            Id = doctorProfileId,
            FullName = "Dr. Emily Johnson",
            About = "Compassionate and dedicated doctor",
            Gender = "Female",
            Email = "emily.johnson@example.com",
            PhotoUrl = "photo3",
            MainInstitutionId = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3305"),
            YearsOfExperience = 2
        };

        var query = new GetDoctorProfileDetialQuery { Id = doctorProfileId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.ShouldBeOfType<Result<DoctorProfileDetailDto>>();
        result.Value.ShouldNotBeNull();
        result.Value.FullName.ShouldBe(expectedDoctorProfile.FullName);
        result.Value.About.ShouldBe(expectedDoctorProfile.About);
        result.Value.Gender.ShouldBe(expectedDoctorProfile.Gender);
        result.Value.Email.ShouldBe(expectedDoctorProfile.Email);
        result.Value.MainInstitutionId.ShouldBe(expectedDoctorProfile.MainInstitutionId);
    }

    [Fact]
    public async Task Handle_NonExistingDoctorProfile_ReturnsNotFoundResult()
    {
        // Arrange
        var nonExistingDoctorProfileId = Guid.NewGuid();

        var query = new GetDoctorProfileDetialQuery { Id = nonExistingDoctorProfileId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Value.ShouldBeNull();
        result.ShouldBeOfType<Result<DoctorProfileDetailDto>>();
    }
}
