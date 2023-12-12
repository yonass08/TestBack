
using Application.Contracts.Persistence;
using Domain;
using Application.Features.Specialities.CQRS.Commands;
using Application.Features.Specialities.CQRS.Handlers;
using Application.Profiles;
using Application.Responses;
using Application.UnitTest.Mocks;
using AutoMapper;
using Moq;
using Shouldly;

namespace Application.UnitTest.SpecialityTest.SpecialityCommandTest;

public class DeleteSpecialityCommandHandlerTest
{
    private IMapper _mapper { get; set; }
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ISpecialityRepository> _mockSpecialityRepository;
    private DeleteSpecialityCommandHandler _handler { get; set; }


    public DeleteSpecialityCommandHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockSpecialityRepository = new Mock<ISpecialityRepository>();

        _mapper = new MapperConfiguration(c =>
      {
          c.AddProfile<MappingProfile>();
      }).CreateMapper();

        _handler = new DeleteSpecialityCommandHandler(_mockUnitOfWork.Object);

    }

    [Fact]
    public async Task DeleteSpecialityValid()
    {
        // Arrange
        var specialityId = Guid.NewGuid();
        var deleteCommand = new DeleteSpecialityCommand { Id = specialityId };
        var specialityToDelete = new Speciality {Id = specialityId};

        
        _mockSpecialityRepository.Setup(r => r.Get(specialityId)).ReturnsAsync(specialityToDelete);
        _mockUnitOfWork.Setup(uow => uow.SpecialityRepository).Returns(_mockSpecialityRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.Save()).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(deleteCommand, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<Guid?>>();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(specialityId);
    }

    [Fact]
    public async Task DeleteSpecialityInvalid()
    {

        var specialityId = Guid.NewGuid();
        var deleteCommand = new DeleteSpecialityCommand { Id = specialityId };

        _mockSpecialityRepository.Setup(r => r.Get(specialityId)).ReturnsAsync((Speciality)null);
        _mockUnitOfWork.Setup(uow => uow.SpecialityRepository).Returns(_mockSpecialityRepository.Object);


        var result = await _handler.Handle(deleteCommand, CancellationToken.None);
        result.Value.ShouldBeEquivalentTo(null);
        result.Error.ShouldBeEquivalentTo("Speciality Not Found.");
        Assert.IsType<Result<Guid?>>(result);
    }
}
