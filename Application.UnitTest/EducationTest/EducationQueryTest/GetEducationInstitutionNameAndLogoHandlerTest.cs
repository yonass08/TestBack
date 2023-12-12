
using Application.UnitTest.Mocks;
using Application.Contracts.Persistence;
using AutoMapper;
using Moq;
using Application.Features.Educations.CQRS.Handlers;
using Application.Profiles;
using Application.Features.Educations.CQRS.Queries;
using Shouldly;
using Application.Responses;
using Domain;
using Application.Features.Educations.DTOs;

namespace Application.UnitTest.EducationTest.EducationQueryTest;

public class GetEducationInstitutionNameAndLogoHandlerTest
{
    private IMapper _mapper { get; set; }
    private Mock<IUnitOfWork> _mockUnitOfWork { get; set; }
    private GetEducationInstitutionNameAndLogoQueryHandler _handler { get; set; }
    public GetEducationInstitutionNameAndLogoHandlerTest()
    {
        _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();

        _mapper = new MapperConfiguration(c =>
        {
            c.AddProfile<MappingProfile>();
        }).CreateMapper();

        _handler = new GetEducationInstitutionNameAndLogoQueryHandler(_mockUnitOfWork.Object, _mapper);
    }


    [Fact]
    public async Task GetEducationListValid()
    {
        var result = await _handler.Handle(new GetEducationInstitutionNameAndLogoQuery(), CancellationToken.None);

        Assert.IsType<Result<List<GetEducationInstitutionNameAndLogoDto>>>(result);
        Assert.True(result.IsSuccess);
    }

}
