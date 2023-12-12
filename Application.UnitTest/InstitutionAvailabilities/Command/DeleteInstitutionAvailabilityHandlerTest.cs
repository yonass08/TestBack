using Application.Contracts.Persistence;
using Application.UnitTest.Mocks;
using Application.Features.InstitutionAvailabilities.CQRS.Commands;
using Application.Features.InstitutionAvailabilities.CQRS.Handlers;
using Application.Features.InstitutionAvailabilities.DTOs;
using Application.Profiles;
using AutoMapper;
using Shouldly;
using Moq;
using Xunit;
using Moq;

namespace Application.UnitTest.InstitutionAvailabilities.Commands
{
    public class DeleteInstitutionAvailabilityCommandHandlerTest
    {
        private IMapper _mapper { get; set; }
       private Mock<IUnitOfWork> _mockUnitOfWork { get; set; }
       private DeleteInstitutionAvailabilityCommandHandler _handler { get; set; }

       public DeleteInstitutionAvailabilityCommandHandlerTest()
       {
              _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
              
              _mapper = new MapperConfiguration(c =>
              {
                     c.AddProfile<MappingProfile>();
              }).CreateMapper();

              _handler = new DeleteInstitutionAvailabilityCommandHandler(_mockUnitOfWork.Object);
       }
       
       
       [Fact]
       public async Task DeleteInstitutionAvailabilityValid()
       {
       
              Guid deleteId = Guid.NewGuid();
              
              var result = await _handler.Handle(new DeleteInstitutionAvailabilityCommand() { Id =  deleteId}, CancellationToken.None);
              
              (await _mockUnitOfWork.Object.InstitutionAvailabilityRepository.GetAll()).Count.ShouldBe(2);
       }
       
       [Fact]
       public async Task DeleteInstitutionAvailabilityInvalid()
       {
       
              Guid deleteId = Guid.NewGuid();
              
              var result = await _handler.Handle(new DeleteInstitutionAvailabilityCommand() { Id =  deleteId}, CancellationToken.None);
              
              (await _mockUnitOfWork.Object.InstitutionAvailabilityRepository.GetAll()).Count.ShouldBe(2);
       }
    }
}