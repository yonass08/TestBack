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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Application.Contracts.Persistence;
using Application.UnitTest.Mocks;
using Application.UnitTest.Mocks;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Application.Contracts.Persistence;
using Application.Features.DoctorAvailabilities.CQRS.Handlers;
using Application.Features.DoctorAvailabilities.CQRS.Queries;
using Application.Features.DoctorAvailabilities.DTOs;
using AutoMapper;
using Moq;
using Xunit;
using Domain;

namespace Application.UnitTest.Features.DoctorAvailabilities.CQRS.Handlers
{
    public class GetDoctorAvailabilityDetailQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ValidQuery_ReturnsDoctorAvailabilityDto()
        {
            // Arrange
            var availabilityId = Guid.NewGuid();
            var availability = new DoctorAvailability
            {
                Id = availabilityId,
                Day = DayOfWeek.Friday,
                    StartTime = "2:00 AM",
                    EndTime = "3:00 PM",
                    DoctorId = Guid.NewGuid(),
                    InstitutionId = Guid.NewGuid(),
                    SpecialityId = Guid.NewGuid(),
            };


            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.DoctorAvailabilityRepository.Get(availabilityId))
                .ReturnsAsync(availability);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<DoctorAvailabilityDto>(availability))
                .Returns(new DoctorAvailabilityDto { Id = availabilityId });

            var handler = new GetDoctorAvailabilityDetailQueryHandler(unitOfWorkMock.Object, mapperMock.Object);
            var query = new GetDoctorAvailabilityDetailQuery { Id = availabilityId };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(availabilityId, result.Value.Id);
        }

        [Fact]
        public async Task Handle_InvalidQuery_ReturnsNull()
        {
            // Arrange
            var availabilityId = Guid.NewGuid();

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.DoctorAvailabilityRepository.Get(availabilityId))
                .ReturnsAsync((DoctorAvailability)null);

            var mapperMock = new Mock<IMapper>();

            var handler = new GetDoctorAvailabilityDetailQueryHandler(unitOfWorkMock.Object, mapperMock.Object);
            var query = new GetDoctorAvailabilityDetailQuery { Id = availabilityId };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}
