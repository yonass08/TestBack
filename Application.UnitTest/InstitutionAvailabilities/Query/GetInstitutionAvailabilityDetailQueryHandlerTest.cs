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
using Application.Features.InstitutionAvailabilities.CQRS.Handlers;
using Application.Features.InstitutionAvailabilities.CQRS.Queries;
using Application.Features.InstitutionAvailabilities.DTOs;
using AutoMapper;
using Moq;
using Xunit;
using Domain;

namespace Application.UnitTest.Features.InstitutionAvailabilities.CQRS.Handlers
{
    public class GetInstitutionAvailabilityDetailQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ValidQuery_ReturnsInstitutionAvailabilityDto()
        {
            // Arrange
            var availabilityId = Guid.NewGuid();
            var availability = new InstitutionAvailability
            {
                Id = availabilityId,
                StartDay = DayOfWeek.Monday,
                EndDay = DayOfWeek.Sunday,
                Opening = "2:00AM",
                Closing = "4:00PM",
                TwentyFourHours = true,
                InstitutionId = Guid.NewGuid()
            };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.InstitutionAvailabilityRepository.Get(availabilityId))
                .ReturnsAsync(availability);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<InstitutionAvailabilityDto>(availability))
                .Returns(new InstitutionAvailabilityDto { Id = availabilityId });

            var handler = new GetInstitutionAvailabilityDetailQueryHandler(unitOfWorkMock.Object, mapperMock.Object);
            var query = new GetInstitutionAvailabilityDetailQuery { Id = availabilityId };

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
            unitOfWorkMock.Setup(uow => uow.InstitutionAvailabilityRepository.Get(availabilityId))
                .ReturnsAsync((InstitutionAvailability)null);

            var mapperMock = new Mock<IMapper>();

            var handler = new GetInstitutionAvailabilityDetailQueryHandler(unitOfWorkMock.Object, mapperMock.Object);
            var query = new GetInstitutionAvailabilityDetailQuery { Id = availabilityId };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}
