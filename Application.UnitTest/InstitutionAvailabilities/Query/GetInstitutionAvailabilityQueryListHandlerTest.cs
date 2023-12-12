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
using System.Collections.Generic;
using System.Threading;
using Application.Features.InstitutionAvailabilities.CQRS.Handlers;
using Application.Features.InstitutionAvailabilities.CQRS.Queries;
using Application.Features.InstitutionAvailabilities.DTOs;
using Application.Responses;
using AutoMapper;
using Moq;
using Xunit;

using System.Collections.Generic;
using System.Threading;
using Application.Features.InstitutionAvailabilities.CQRS.Handlers;
using Application.Features.InstitutionAvailabilities.CQRS.Queries;
using Application.Features.InstitutionAvailabilities.DTOs;
using Application.Responses;
using AutoMapper;
using Moq;
using Xunit;

namespace Application.UnitTest.Features.InstitutionAvailabilities.CQRS.Handlers
{
    public class GetInstitutionAvailabilityListQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ValidQuery_ReturnsSuccessResultWithInstitutionAvailabilityList()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var expectedInstitutionAvailabilities = new List<Domain.InstitutionAvailability>
            {
                new Domain.InstitutionAvailability
                {
                    StartDay = DayOfWeek.Monday,
                    EndDay = DayOfWeek.Sunday,
                    Opening = "2:00AM", // Updated property name
                    Closing = "4:00PM", // Updated property name
                    TwentyFourHours = true,
                    InstitutionId = Guid.NewGuid(),
                    Id = Guid.NewGuid()
                },
                new Domain.InstitutionAvailability
                {
                    StartDay = DayOfWeek.Monday,
                    EndDay = DayOfWeek.Sunday,
                    Opening = "1:00AM", // Updated property name
                    Closing = "7:00PM", // Updated property name
                    TwentyFourHours = false,
                    InstitutionId = Guid.NewGuid(),
                    Id = Guid.NewGuid()
                }
            };

            var expectedDtoList = new List<InstitutionAvailabilityDto>
            {
                new InstitutionAvailabilityDto
                {
                    StartDay = "Monday",
                    EndDay = "Sunday",
                    Opening = "2:00AM",
                    Closing = "4:00PM",
                    TwentyFourHours = true
                },
                new InstitutionAvailabilityDto
                {
                    StartDay = "Monday",
                    EndDay = "Sunday",
                    Opening = "1:00AM",
                    Closing = "7:00PM",
                    TwentyFourHours = false
                }
            };

            unitOfWorkMock.Setup(uow => uow.InstitutionAvailabilityRepository.GetAll())
                .ReturnsAsync(expectedInstitutionAvailabilities);
            mapperMock.Setup(mapper => mapper.Map<List<InstitutionAvailabilityDto>>(expectedInstitutionAvailabilities))
                .Returns(expectedDtoList);

            var handler = new GetInstitutionAvailabilityListQueryHandler(unitOfWorkMock.Object, mapperMock.Object);
            var query = new GetInstitutionAvailabilityListQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedDtoList, result.Value);
            Assert.Null(result.Error);
        }
    }
}
