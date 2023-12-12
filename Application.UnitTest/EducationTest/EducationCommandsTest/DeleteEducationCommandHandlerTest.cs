using Application.Contracts.Persistence;
using Application.Features.Educations.CQRS;
using Application.Features.Educations.CQRS.Handlers;
using Application.Features.Educations.DTOs;
using Application.Interfaces;
using Application.Photos;
using Application.Profiles;
using Application.Responses;
using Application.UnitTest.Mocks;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;

namespace Application.UnitTest.EducationTest.EducationCommandsTest;

public class DeleteEducationCommandHandlerTest
{
    private  IMapper _mapper { get; set; }
    private Mock<IUnitOfWork> _mockUnitOfWork { get; set; }

    private Mock<IEducationRepository> _mockEducationRepository { get; set; }
    private DeleteEducationCommandHandler _handler { get; set; }


    public DeleteEducationCommandHandlerTest()
    {
        _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
        _mockEducationRepository = new Mock<IEducationRepository>();

        _mapper = new MapperConfiguration(c =>
      {
          c.AddProfile<MappingProfile>();
      }).CreateMapper();

        _handler = new DeleteEducationCommandHandler(_mockUnitOfWork.Object, _mapper);
}


    [Fact]
    public async Task DeleteEducationValid()
    {
        var educationId = Guid.NewGuid();
        var deleteCommand = new DeleteEducationCommand { Id = educationId };
        var specialityToDelete = new Education { Id = educationId };

        _mockEducationRepository.Setup(r => r.Get(educationId)).ReturnsAsync(specialityToDelete);
        _mockUnitOfWork.Setup(uow => uow.EducationRepository).Returns(_mockEducationRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.Save()).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(deleteCommand, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<Guid?>>();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(educationId);
    }


    [Fact]
    public async Task DeleteEducationInvalid()
    {
        var result = await _handler.Handle(new DeleteEducationCommand() { Id = Guid.NewGuid() }, CancellationToken.None);
        result.Value.ShouldBeEquivalentTo(null);
        result.Error.ShouldBeEquivalentTo("Education Not Found.");
        Assert.IsType<Result<Guid?>>(result);
    }
}
