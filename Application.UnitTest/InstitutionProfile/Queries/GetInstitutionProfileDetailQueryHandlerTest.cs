using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Application.UnitTest.Mocks;
using Application.Contracts.Persistence;
using Application.Features.InstitutionProfiles.CQRS.Handlers;
using Application.Features.InstitutionProfiles.CQRS.Queries;
using Application.Features.InstitutionProfiles.DTOs;
using Application.Responses;
using Domain;
using Xunit;

namespace Application.UnitTest.InstitutionProfiles.Queries
{
    public class GetInstitutionProfileDetailQueryHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly GetInstitutionProfileDetailQueryHandler _handler;

        public GetInstitutionProfileDetailQueryHandlerTest()
        {
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<InstitutionProfile, InstitutionProfileDetailDto>();
                cfg.CreateMap<Education, EducationalInstitutionDto>();
            }).CreateMapper();

            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
            _handler = new GetInstitutionProfileDetailQueryHandler(_mockUnitOfWork.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ValidId_ReturnsInstitutionProfileDetailDto()
        {
            // Arrange
            var institutionProfileId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7");
            var institutionProfile = new InstitutionProfile
            {
                Id = institutionProfileId,
                InstitutionName = "Sample Institution"
            };
            

            var query = new GetInstitutionProfileDetailQuery { Id = institutionProfileId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Result<InstitutionProfileDetailDto>>(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.IsType<InstitutionProfileDetailDto>(result.Value);
            //TODO: Perform additional assertions
        }

        [Fact]
        public async Task Handle_NonExistingId_ReturnsNull()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            var query = new GetInstitutionProfileDetailQuery { Id = nonExistingId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Result<InstitutionProfileDetailDto>>(result);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
        }
    }
}
