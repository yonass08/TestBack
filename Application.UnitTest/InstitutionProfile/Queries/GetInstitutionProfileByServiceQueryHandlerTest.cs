using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Application.Contracts.Persistence;
using Application.Features.InstitutionProfiles.CQRS.Handlers;
using Application.Features.InstitutionProfiles.CQRS.Queries;
using Application.Features.InstitutionProfiles.DTOs;
using Application.Responses;
using Domain;
using Xunit;

namespace Application.UnitTest.InstitutionProfiles.Queries
{
    public class GetInstitutionProfileByServiceQueryHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly GetInstitutionProfileByServiceQueryHandler _handler;

        public GetInstitutionProfileByServiceQueryHandlerTest()
        {
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<InstitutionProfile, InstitutionProfileDto>();
                // Add mappings for other types if needed
            }).CreateMapper();

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new GetInstitutionProfileByServiceQueryHandler(_mockUnitOfWork.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ValidServiceId_ReturnsInstitutionProfileDtoList()
        {
            // Arrange
            var serviceId = Guid.NewGuid();
            var institutionProfiles = new List<InstitutionProfile>
            {
                new InstitutionProfile
                {
                    Id = Guid.NewGuid(),
                    InstitutionName = "Institution 1",
                    // Set other properties accordingly
                },
                new InstitutionProfile
                {
                    Id = Guid.NewGuid(),
                    InstitutionName = "Institution 2",
                    // Set other properties accordingly
                }
            };
            _mockUnitOfWork.Setup(uow => uow.InstitutionProfileRepository.GetByService(serviceId))
                .ReturnsAsync(institutionProfiles);

            var query = new GetInstitutionProfileByServiceQuery { ServiceId = serviceId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Result<List<InstitutionProfileDto>>>(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.IsType<List<InstitutionProfileDto>>(result.Value);
            Assert.Equal(institutionProfiles.Count, result.Value.Count);
            // Perform additional assertions if needed
        }

        [Fact]
        public async Task Handle_NonExistingServiceId_ReturnsNull()
        {
            // Arrange
            var nonExistingServiceId = Guid.NewGuid();
            _mockUnitOfWork.Setup(uow => uow.InstitutionProfileRepository.GetByService(nonExistingServiceId))
                .ReturnsAsync((List<InstitutionProfile>)null);

            var query = new GetInstitutionProfileByServiceQuery { ServiceId = nonExistingServiceId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Result<List<InstitutionProfileDto>>>(result);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
        }
    }
}
