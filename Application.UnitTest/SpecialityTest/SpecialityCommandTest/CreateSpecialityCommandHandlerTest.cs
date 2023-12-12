using Application.Contracts.Persistence;
using Application.Features.Specialities.DTOs;
using Application.UnitTest.Mocks;
using AutoMapper;
using Domain;
using Moq;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Application.Features.Specialities.CQRS.Handlers;
using Application.Profiles;
using Application.Features.Specialities.CQRS.Commands;

namespace Application.UnitTest.SpecialityTest.SpecialityCommandsTest;
    public class CreateSpecialityCommandHandlerTest
    {
        private IMapper _mapper;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly CreateSpecialityDto createSpecialityDto;
        private CreateSpecialityCommandHandler _handler;

        public CreateSpecialityCommandHandlerTest()
        {
            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();

            _mapper = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            }).CreateMapper();

            createSpecialityDto = new CreateSpecialityDto()
            {
                Id = Guid.NewGuid(),
                Name = "Cardiology",
                Description = "Specialty in heart diseases",
    
            };

            _handler = new CreateSpecialityCommandHandler(_mockUnitOfWork.Object, _mapper);
        }

        [Fact]
        public async Task CreateSpecialityValid()
        {
            var result = await _handler.Handle(new CreateSpecialityCommand() { SpecialityDto = createSpecialityDto }, CancellationToken.None);
            Assert.IsType<CreateSpecialityDto>(result.Value);
            result.IsSuccess.ShouldBeTrue();
            var repo = await _mockUnitOfWork.Object.SpecialityRepository.GetAll();
            repo.Count.ShouldBe(3);
        }

        [Fact]
        public async Task CreateSpecialityInvalid()
        {
            
            createSpecialityDto.Name = null;
            var command = new CreateSpecialityCommand() { SpecialityDto = createSpecialityDto };
            var result = await _handler.Handle(command, CancellationToken.None);
            result.Value.ShouldBe(null);
            result.IsSuccess.ShouldBeFalse();
        }
    }

