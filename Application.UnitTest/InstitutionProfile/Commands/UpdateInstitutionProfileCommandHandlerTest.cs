using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.Persistence;
using Application.Features.InstitutionProfiles.CQRS.Commands;
using Application.Features.InstitutionProfiles.CQRS.Handlers;
using Application.Features.InstitutionProfiles.DTOs;
using Application.Features.InstitutionProfiles.DTOs.Validators;
using Application.UnitTest.Mocks;
using Application.Interfaces;
using Application.Responses;
using Microsoft.AspNetCore.Http;
using Application.Photos;
using AutoMapper;
using Domain;
using Moq;
using Shouldly;
using Xunit;

namespace Application.UnitTest.InstitutionProfiles.Commands
{
    public class UpdateInstitutionProfileCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IPhotoAccessor> photoAccessorMock;
        private readonly UpdateInstitutionProfileCommandHandler handler;

        public UpdateInstitutionProfileCommandHandlerTests()
        {
            unitOfWorkMock = MockUnitOfWork.GetUnitOfWork();
            mapperMock = new Mock<IMapper>();
            photoAccessorMock = new Mock<IPhotoAccessor>();
            handler = new UpdateInstitutionProfileCommandHandler(unitOfWorkMock.Object, mapperMock.Object, photoAccessorMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsSuccessResult()
        {
            // Arrange
            var institutionProfileDto = new UpdateInstitutionProfileDto
            {
                Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                InstitutionName = "Institution 1",
                BranchName = "Branch 1",
                Website = "www.Website.com",
                PhoneNumber = "Phone 1",
                Summary = "Summary 1",
                EstablishedOn = DateTime.Now.AddDays(-10),
                Rate = 4.5
            };
            var command = new UpdateInstitutionProfileCommand
            {
                UpdateInstitutionProfileDto = institutionProfileDto
            };

            var photoUploadResult = new PhotoUploadResult
            {
                PublicId = "1",
                Url = "test_image.jpg"
            };

            photoAccessorMock.Setup(x => x.AddPhoto(It.IsAny<IFormFile>())).ReturnsAsync(photoUploadResult);

            var institutionProfile = new InstitutionProfile { Id = institutionProfileDto.Id };
            mapperMock.Setup(x => x.Map<InstitutionProfile>(institutionProfileDto)).Returns(institutionProfile);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            unitOfWorkMock.Verify(x => x.Save(), Times.Exactly(1));
        }

        [Fact]
        public async Task Handle_InvalidCommand_ReturnsFailureResult()
        {
            // Arrange
            var institutionProfileDto = new UpdateInstitutionProfileDto
            {
                Id = Guid.NewGuid(),
                InstitutionName = "Institution 1",
                BranchName = "Branch 1",
                Website = "Website",
                PhoneNumber = "Phone 1",
                Summary = "Summary 1",
                EstablishedOn = DateTime.Now.AddDays(-10),
                Rate = 4.5
            };
            var command = new UpdateInstitutionProfileCommand
            {
                UpdateInstitutionProfileDto = institutionProfileDto
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldNotBeEmpty();
            unitOfWorkMock.Verify(x => x.Save(), Times.Never());
        }
    }
}
