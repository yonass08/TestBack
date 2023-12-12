using Application.UnitTest.Mocks;
using Application.Contracts.Persistence;
using AutoMapper;
using Moq;
using Xunit;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Specialities.CQRS.Queries;
using Application.Features.Specialities.DTOs;
using Application.Responses;
using Domain;
using System.Collections.Generic;
using Application.Profiles;
using Application.Features.Specialities.CQRS.Handlers;

namespace Application.UnitTest.SpecialityTest.SpecialityQueryTest
{
    public class GetSpecialityListQueryHandlerTest
    {
        private IMapper _mapper;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private GetSpecialityListQueryHandler _handler;

        public GetSpecialityListQueryHandlerTest()
        {
            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();

            _mapper = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            }).CreateMapper();

            _handler = new GetSpecialityListQueryHandler(_mockUnitOfWork.Object, _mapper);
        }

        [Fact]
        public async Task GetSpecialityListValid()
        {
            // Arrange
            var specialityList = new List<Speciality>
            {
                new Speciality
                {
                    Id = Guid.NewGuid(),
                    Name = "Cardiology",
                    Description = "Specialty dealing with heart diseases"
                },
                new Speciality
                {
                    Id = Guid.NewGuid(),
                    Name = "Dermatology",
                    Description = "Specialty dealing with skin diseases"
                }
            };

            _mockUnitOfWork.Setup(uow => uow.SpecialityRepository.GetAll()).ReturnsAsync(specialityList);

            var query = new GetSpecialityListQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<Result<List<SpecialityDto>>>();
            result.IsSuccess.ShouldBeTrue();
            result.Error.ShouldBeNull();
            result.Value.Count.ShouldBe(2);
        }
    }
}
