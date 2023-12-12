using Application.Contracts.Persistence;
using Application.Features.Educations.CQRS.Handlers;
using Application.Features.Educations.CQRS.Queries;
using Application.Features.Educations.DTOs;
using Application.Profiles;
using Application.UnitTest.Mocks;
using AutoMapper;
using Domain;
using Moq;
using Xunit;
using Shouldly;
using Application.Responses;

namespace Application.UnitTest.EducationTest.EducationQueryTest;

public class GetEducationDetailCommandHandlerTest
{
    private Mock<IMapper> _mapper { get; set; }
    private Mock<IUnitOfWork> _mockUnitOfWork { get; set; }
    private GetEducationByIdQueryHandler _handler { get; set; }

    public GetEducationDetailCommandHandlerTest()
    {
        _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
        _mapper = new Mock<IMapper>();
        _handler = new GetEducationByIdQueryHandler(_mockUnitOfWork.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetEducationDetailsValid()
    {
        var educationId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7");
        Education education = new(){
            Id = educationId,
            EducationInstitution = "Addis Ababa University",
            StartYear = DateTime.Today,
            GraduationYear = DateTime.Today,
            Degree = "Masters",
            DoctorId = Guid.NewGuid(),
            EducationInstitutionLogoId = "Addis Ababa Logo"
        };
        EducationDto educationDto = new()
        {
            Id = educationId,
            EducationInstitution = "Addis Ababa University",
            StartYear = DateTime.Today,
            GraduationYear = DateTime.Today
        };

        _mockUnitOfWork.Setup(uow => uow.EducationRepository.GetPopulated(educationId)).ReturnsAsync(education);
        _mapper.Setup(mapper => mapper.Map<EducationDto>(education)).Returns(educationDto);
        var result = await _handler.Handle(new GetEducationDetailQuery() { Id = educationDto.Id }, CancellationToken.None);
        result.ShouldNotBe(null);
        Assert.IsType<Result<EducationDto>>(result);

    }

    [Fact]
    public async Task GetEducationDetailsInvalid()
    {
        var result = await _handler.Handle(new GetEducationDetailQuery() { Id = Guid.NewGuid() }, CancellationToken.None);
        result.Value.ShouldBe(null);
        Assert.Equal("Fetch Failed", result.Error);
    }
}


