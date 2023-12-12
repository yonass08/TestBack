using AutoMapper;
using Application.Contracts.Persistence;
using Application.Features.InstitutionProfiles.CQRS.Handlers;
using Application.Features.InstitutionProfiles.CQRS.Queries;
using Application.Features.InstitutionProfiles.DTOs;
using Application.Responses;
using Domain;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTest.InstitutionProfiles.Queries
{
    public class GetInstitutionProfileByOperationYearsQueryHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly GetInstitutionProfileByOprationYearsQueryHandler _handler;

        public GetInstitutionProfileByOperationYearsQueryHandlerTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.CreateMap<InstitutionProfile, InstitutionProfileDto>();
            });
            _mapper = mapperConfig.CreateMapper();
            _handler = new GetInstitutionProfileByOprationYearsQueryHandler(_mockUnitOfWork.Object, _mapper);
        }

        [Fact]
        public async Task GetInstitutionProfileByOperationYears_ShouldReturnListOfInstitutionProfiles()
        {
            // Arrange
            var years = 5;

            var institutionProfiles = new List<InstitutionProfile>
            {
                new InstitutionProfile { Id = Guid.NewGuid(), InstitutionName = "Institution 1", EstablishedOn = DateTime.Now.AddYears(-3) },
                new InstitutionProfile { Id = Guid.NewGuid(), InstitutionName = "Institution 2", EstablishedOn = DateTime.Now.AddYears(-4) },
                new InstitutionProfile { Id = Guid.NewGuid(), InstitutionName = "Institution 3", EstablishedOn = DateTime.Now.AddYears(-6) }
            };

            _mockUnitOfWork.Setup(uow => uow.InstitutionProfileRepository.GetByYears(years))
                .ReturnsAsync(institutionProfiles);

            var query = new GetInstitutionProfileByOprationYearsQuery { Years = years };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<Result<List<InstitutionProfileDto>>>(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Value.Count);

            _mockUnitOfWork.Verify(uow => uow.InstitutionProfileRepository.GetByYears(years), Times.Once);
        }

        [Fact]
        public async Task GetInstitutionProfileByOperationYears_EmptyList_ShouldReturnEmptyResult()
        {
            // Arrange
            var years = 5;

            List<InstitutionProfile> institutionProfiles = new List<InstitutionProfile>();

            _mockUnitOfWork.Setup(uow => uow.InstitutionProfileRepository.GetByYears(years))
                .ReturnsAsync(institutionProfiles);

            var query = new GetInstitutionProfileByOprationYearsQuery { Years = years };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<Result<List<InstitutionProfileDto>>>(result);
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value);

            _mockUnitOfWork.Verify(uow => uow.InstitutionProfileRepository.GetByYears(years), Times.Once);
        }
    }
}
