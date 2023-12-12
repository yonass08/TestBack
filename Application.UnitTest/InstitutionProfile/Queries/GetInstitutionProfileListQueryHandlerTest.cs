using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.Persistence;
using Application.Features.InstitutionProfiles.DTOs;
using Application.Features.InstitutionProfiles.CQRS.Handlers;
using Application.Features.Specialities.CQRS.Queries;
using Application.Features.Specialities.DTOs;
using Application.Responses;
using AutoMapper;
using Domain;
using Moq;
using Xunit;

namespace Application.UnitTest.InstitutionProfiles.Queries
{
    public class GetInstitutionProfileListQueryHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly GetInstitutionProfileListQueryHandler _handler;

        public GetInstitutionProfileListQueryHandlerTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.CreateMap<InstitutionProfile, InstitutionProfileDto>();
            });
            _mapper = mapperConfig.CreateMapper();
            _handler = new GetInstitutionProfileListQueryHandler(_mockUnitOfWork.Object, _mapper);
        }

        [Fact]
        public async Task GetInstitutionProfileList_ShouldReturnListOfInstitutionProfiles()
        {
            // Arrange
            var institutionProfiles = new List<InstitutionProfile>
            {
                new InstitutionProfile { Id = Guid.NewGuid(), InstitutionName = "Institution 1" },
                new InstitutionProfile { Id = Guid.NewGuid(), InstitutionName = "Institution 2" }
            };

            _mockUnitOfWork.Setup(uow => uow.InstitutionProfileRepository.GetAllPopulated()).ReturnsAsync(institutionProfiles);

            var query = new GetInstitutionProfileListQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<Result<List<InstitutionProfileDto>>>(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(institutionProfiles.Count, result.Value.Count);

            _mockUnitOfWork.Verify(uow => uow.InstitutionProfileRepository.GetAllPopulated(), Times.Once);
        }

        [Fact]
        public async Task GetInstitutionProfileList_EmptyList_ShouldReturnEmptyResult()
        {
            // Arrange
            List<InstitutionProfile> institutionProfiles = null;

            _mockUnitOfWork.Setup(uow => uow.InstitutionProfileRepository.GetAllPopulated()).ReturnsAsync(institutionProfiles);

            var query = new GetInstitutionProfileListQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<Result<List<InstitutionProfileDto>>>(result);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);

            _mockUnitOfWork.Verify(uow => uow.InstitutionProfileRepository.GetAllPopulated(), Times.Once);
        }
    }
}
