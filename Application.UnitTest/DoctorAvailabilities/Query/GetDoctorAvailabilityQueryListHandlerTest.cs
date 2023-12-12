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
using System.Collections.Generic;
using System.Threading;
using Application.Features.DoctorAvailabilities.CQRS.Handlers;
using Application.Features.DoctorAvailabilities.CQRS.Queries;
using Application.Features.DoctorAvailabilities.DTOs;
using Application.Responses;
using AutoMapper;
using Moq;
using Xunit;

using System.Collections.Generic;
using System.Threading;
using Application.Features.DoctorAvailabilities.CQRS.Handlers;
using Application.Features.DoctorAvailabilities.CQRS.Queries;
using Application.Features.DoctorAvailabilities.DTOs;
using Application.Responses;
using AutoMapper;
using Moq;
using Xunit;

namespace Application.UnitTest.Features.DoctorAvailabilities.CQRS.Handlers
{
    public class GetDoctorAvailabilityListQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ValidQuery_ReturnsSuccessResultWithDoctorAvailabilityList()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();

            var expectedDoctorAvailabilities = new List<Domain.DoctorAvailability>
            {
                new Domain.DoctorAvailability
                {
                    Day = DayOfWeek.Friday,
                    StartTime = "2:00 AM",
                    EndTime = "3:00 PM",
                    DoctorId = Guid.NewGuid(),
                    InstitutionId = Guid.NewGuid(),
                    SpecialityId = Guid.NewGuid(),
                    Id = Guid.NewGuid()
                },
                new Domain.DoctorAvailability
                {
                    Day = DayOfWeek.Friday,
                    StartTime = "2:00 AM",
                    EndTime = "3:00 PM",
                    DoctorId = Guid.NewGuid(),
                    InstitutionId = Guid.NewGuid(),
                    SpecialityId = Guid.NewGuid(),
                    Id = Guid.NewGuid()
                }
            };

            var expectedDtoList = new List<DoctorAvailabilityDto>
            {
                new DoctorAvailabilityDto
                {
                    Day = DayOfWeek.Friday,
                    StartTime = "2:00 AM",
                    EndTime = "3:00 PM",
                    DoctorId = Guid.NewGuid(),
                    InstitutionId = Guid.NewGuid(),
                    SpecialityId = Guid.NewGuid(),
                },
                new DoctorAvailabilityDto
                {
                    Day = DayOfWeek.Friday,
                    StartTime = "2:00 AM",
                    EndTime = "3:00 PM",
                    DoctorId = Guid.NewGuid(),
                    InstitutionId = Guid.NewGuid(),
                    SpecialityId = Guid.NewGuid(),
                }
            };

            unitOfWorkMock.Setup(uow => uow.DoctorAvailabilityRepository.GetAll())
                .ReturnsAsync(expectedDoctorAvailabilities);
            mapperMock.Setup(mapper => mapper.Map<List<DoctorAvailabilityDto>>(expectedDoctorAvailabilities))
                .Returns(expectedDtoList);

            var handler = new GetDoctorAvailabilityListQueryHandler(unitOfWorkMock.Object, mapperMock.Object);
            var query = new GetDoctorAvailabilityListQuery();

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
