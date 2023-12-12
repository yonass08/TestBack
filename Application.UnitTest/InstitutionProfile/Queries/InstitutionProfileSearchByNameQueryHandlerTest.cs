using AutoMapper;
using Application.Contracts.Persistence;
using Application.Features.InstitutionProfiles.CQRS.Handlers;
using Application.Features.Specialities.CQRS.Queries;
using Application.Features.InstitutionProfiles.DTOs;
using Domain;
using Application.Responses;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTest.InstitutionProfiles.Queries
{
    public class InstitutionProfileSearchByNameQueryHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly InstitutionProfileSearchByNameQueryHandler _handler;

        public InstitutionProfileSearchByNameQueryHandlerTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.CreateMap<InstitutionProfile, InstitutionProfileDto>();
            });
            _mapper = mapperConfig.CreateMapper();
            _handler = new InstitutionProfileSearchByNameQueryHandler(_mockUnitOfWork.Object, _mapper);
        }

        [Fact]
        public async Task InstitutionProfileSearchByNameQuery_ShouldReturnListOfInstitutionProfiles()
        {
            // Arrange
            var query = new InstitutionProfileSearchByNameQuery { Name = "Institution" };

            var institutionProfiles = new List<InstitutionProfile>
            {
                new InstitutionProfile { Id = Guid.NewGuid(), InstitutionName = "Institution 1" },
                new InstitutionProfile { Id = Guid.NewGuid(), InstitutionName = "Institution 2" },
                new InstitutionProfile { Id = Guid.NewGuid(), InstitutionName = "Institution 3" }
            };

            _mockUnitOfWork.Setup(uow => uow.InstitutionProfileRepository.Search(query.Name))
                .ReturnsAsync(institutionProfiles);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<Result<List<InstitutionProfileDto>>>(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Value.Count);

            _mockUnitOfWork.Verify(uow => uow.InstitutionProfileRepository.Search(query.Name), Times.Once);
        }

        [Fact]
        public async Task InstitutionProfileSearchByNameQuery_ShouldReturnEmptyResult()
        {
            // Arrange
            var query = new InstitutionProfileSearchByNameQuery { Name = "NonExistingInstitution" };
            var institutionProfiles = new List<InstitutionProfile>();
            
            _mockUnitOfWork.Setup(uow => uow.InstitutionProfileRepository.Search(query.Name))
                .ReturnsAsync(institutionProfiles);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<Result<List<InstitutionProfileDto>>>(result);
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value);

            _mockUnitOfWork.Verify(uow => uow.InstitutionProfileRepository.Search(query.Name), Times.Once);
        }
    }
}
