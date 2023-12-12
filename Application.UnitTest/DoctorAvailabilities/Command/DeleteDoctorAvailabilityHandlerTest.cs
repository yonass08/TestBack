using Application.Contracts.Persistence;
using Application.UnitTest.Mocks;
using Application.Features.DoctorAvailabilities.CQRS.Commands;
using Application.Features.DoctorAvailabilities.CQRS.Handlers;
using Application.Features.DoctorAvailabilities.DTOs;
using Application.Profiles;
using AutoMapper;
using Shouldly;
using Moq;
using Xunit;
using Moq;

namespace Application.UnitTest.DoctorAvailabilities.Commands
{
    public class DeleteDoctorAvailabilityCommandHandlerTest
    {
        private IMapper _mapper { get; set; }
       private Mock<IUnitOfWork> _mockUnitOfWork { get; set; }
       private DeleteDoctorAvailabilityCommandHandler _handler { get; set; }

       public DeleteDoctorAvailabilityCommandHandlerTest()
       {
              _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
              
              _mapper = new MapperConfiguration(c =>
              {
                     c.AddProfile<MappingProfile>();
              }).CreateMapper();

              _handler = new DeleteDoctorAvailabilityCommandHandler(_mockUnitOfWork.Object);
       }
       
       
       [Fact]
       public async Task DeleteDoctorAvailabilityValid()
       {
       
              Guid deleteId = Guid.NewGuid();
              
              var result = await _handler.Handle(new DeleteDoctorAvailabilityCommand() { Id =  deleteId}, CancellationToken.None);
              
              (await _mockUnitOfWork.Object.DoctorAvailabilityRepository.GetAll()).Count.ShouldBe(2);
       }
       
       [Fact]
       public async Task DeleteDoctorAvailabilityInvalid()
       {
       
              Guid deleteId = Guid.NewGuid();
              
              var result = await _handler.Handle(new DeleteDoctorAvailabilityCommand() { Id =  deleteId}, CancellationToken.None);
              
              (await _mockUnitOfWork.Object.DoctorAvailabilityRepository.GetAll()).Count.ShouldBe(2);
       }
    }
}