using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using AutoMapper;
using Moq;
using Shouldly;
using Application.Contracts.Persistence;
using Application.Features.InstitutionProfiles.CQRS.Handlers;
using Application.Features.InstitutionProfiles.CQRS.Queries;
using Application.Features.InstitutionProfiles.DTOs;
using Application.Responses;
using Domain;
using Xunit;

namespace Application.UnitTest.InstitutionProfiles.Queries
{
    public class InstitutionProfileSearchQueryHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly InstitutionProfileSearchQueryHandler _handler;

        public InstitutionProfileSearchQueryHandlerTest()
        {
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<InstitutionProfile, InstitutionProfileDto>();
                // Add mappings for other types if needed
            }).CreateMapper();

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new InstitutionProfileSearchQueryHandler(_mockUnitOfWork.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ValidQuery_ReturnsInstitutionProfileDtoList()
        {
            // Arrange
            var query = new InstitutionProfileSearchQuery
            {
                ServiceNames = new List<string> { "Sample Service", "Sample Service 2", "Sample Service 3" },
                OperationYears = 5,
                OpenStatus = true,
                Name = "Sung"
            };

            var institutionProfiles = new List<InstitutionProfile>
            {
                new InstitutionProfile
                {
                    Id = Guid.NewGuid(),
                    InstitutionName = "Institution 1"
                },
                new InstitutionProfile
                {
                    Id = Guid.NewGuid(),
                    InstitutionName = "Institution 2"
                }
            };

            _mockUnitOfWork.Setup(uow => uow.InstitutionProfileRepository.Search(query.ServiceNames, query.OperationYears, query.OpenStatus, query.Name, query.pageNumber, query.pageSize, query.latitude, query.longitude, query.maxDistance))

                .ReturnsAsync(institutionProfiles);

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
        public async Task Handle_EmptyQuery_ReturnsNull()
        {
            // Arrange
            var query = new InstitutionProfileSearchQuery();
            var institutionProfiles = new List<InstitutionProfile>();

            _mockUnitOfWork.Setup(uow => uow.InstitutionProfileRepository.Search(query.ServiceNames, query.OperationYears, query.OpenStatus, query.Name, query.pageNumber, query.pageSize, query.latitude, query.longitude, query.maxDistance))
                .ReturnsAsync(institutionProfiles);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Result<List<InstitutionProfileDto>>>(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_WithPagination_ReturnsPagedInstitutionProfiles()
        {
            // Arrange
            var query = new InstitutionProfileSearchQuery
            {
                pageNumber = 2,
                pageSize = 2
            };


            var ExpectedinstitutionProfiles = new List<InstitutionProfile>
            {
                new InstitutionProfile
                {
                    Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7"),
                    InstitutionName = "Institution 2",
                    BranchName = "Branch 2",
                    Website = "www.Website.com",
                    PhoneNumber = "Phone 2",
                    Summary = "Summary 2",
                    EstablishedOn = DateTime.Now.AddDays(-10),
                    Rate = 3.8,
                    LogoId = "LogoId 2",
                    BannerId = "BannerId 2"
                }
            };

            _mockUnitOfWork.Setup(uow => uow.InstitutionProfileRepository.Search(query.ServiceNames, query.OperationYears, query.OpenStatus, query.Name, query.pageNumber, query.pageSize, query.latitude, query.longitude, query.maxDistance))

               .ReturnsAsync(ExpectedinstitutionProfiles);

            //act
            var result = await _handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<Result<List<InstitutionProfileDto>>>(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.IsType<List<InstitutionProfileDto>>(result.Value);
            Assert.Equal(ExpectedinstitutionProfiles.Count, result.Value.Count);


        }
        
    }
}
