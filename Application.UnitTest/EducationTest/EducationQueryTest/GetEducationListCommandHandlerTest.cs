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

public class GetEducationListCommandHandlerTest
{
    private IMapper _mapper { get; set; }
    private Mock<IUnitOfWork> _mockUnitOfWork { get; set; }
    private GetEducationListQueryHandler _handler { get; set; }
    public GetEducationListCommandHandlerTest()
    {
        _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();

        _mapper = new MapperConfiguration(c =>
        {
            c.AddProfile<MappingProfile>();
        }).CreateMapper();

        _handler = new GetEducationListQueryHandler(_mockUnitOfWork.Object, _mapper);
    }


    [Fact]
    public async Task GetEducationListValid()
    {
        var result = await _handler.Handle(new GetEducationListQuery(), CancellationToken.None);
        result.Value.Count.ShouldBe(2);
        Assert.IsType<Result<List<EducationDto>>>(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(result.Value.Count, result.Value.Count);
    }

}
