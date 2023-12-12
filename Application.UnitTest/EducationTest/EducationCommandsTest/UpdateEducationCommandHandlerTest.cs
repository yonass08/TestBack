using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.Persistence;
using Application.Features.Educations.CQRS;
using Application.Features.Educations.CQRS.Handlers;
using Application.Features.Educations.DTOs;
using Application.Interfaces;
using Application.Photos;
using Application.Profiles;
using Application.Responses;
using Application.UnitTest.Mocks;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;
using Xunit;

namespace Application.UnitTest.EducationTest.EducationCommandsTest
{
    public class UpdateEducationCommandHandlerTest
    {
        private Mock<IMapper> _mapper { get; set; }
        private Mock<IPhotoAccessor> _photoAccesor { get; set; }
        private readonly UpdateEducationCommandHandler _handler;
        private Mock<IUnitOfWork> _mockUnitOfWork { get; set; }


        public UpdateEducationCommandHandlerTest()
        {
            _photoAccesor = new Mock<IPhotoAccessor>();
            _mapper = new Mock<IMapper>();
        _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();

        _handler = new UpdateEducationCommandHandler(_mockUnitOfWork.Object, _mapper.Object, _photoAccesor.Object);
        }

        [Fact]
        public async Task UpdateEducationValid()
        {
            var photo = new PhotoUploadResult { PublicId = Guid.NewGuid().ToString(), Url = "photo-public-id" };
            _photoAccesor.Setup(pa => pa.AddPhoto(It.IsAny<IFormFile>())).ReturnsAsync(photo);

            
            var updateEducationDto = new UpdateEducationDto
            {
                Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7"),
                EducationInstitution = "Updated University",
                StartYear = DateTime.Now.AddYears(-2),
                GraduationYear = DateTime.Now.AddYears(-1),
                Degree = "Master's",
                FieldOfStudy = "Psychiatry",
                DoctorId = Guid.NewGuid(),
                EducationInstitutionLogoPhotoId = "Updated Campus"
            };
           
            var education = new Education { Id = updateEducationDto.Id };
            _mapper.Setup(x => x.Map<Education>(updateEducationDto)).Returns(education);

            // Act
            var result = await _handler.Handle(new UpdateEducationCommand{
                updateEducationDto = updateEducationDto
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Value.ShouldBe(Unit.Value);
            result.Error.ShouldBe("Education Updated Successfully.");
            _mockUnitOfWork.Verify(x => x.Save(), Times.Exactly(1));
            
        }

        [Fact]
        public async Task UpdateEducationInvalid_EducationNotFound()
        {

            var updateEducationDto = new UpdateEducationDto
            {
                Id = Guid.NewGuid(),
                EducationInstitution = "Updated University no to be found",
                StartYear = DateTime.Now.AddYears(-2),
                GraduationYear = DateTime.Now.AddYears(-1),
                Degree = "Master's",
                FieldOfStudy = "Psychiatry",
                DoctorId = Guid.NewGuid(),
                EducationInstitutionLogoPhotoId = "Updated Campus"
            };

            var result = await _handler.Handle(new UpdateEducationCommand(){updateEducationDto = updateEducationDto}, CancellationToken.None);

            result.IsSuccess.ShouldBeFalse();
            result.Error.ShouldBe("Education Not Found.");
            result.Value.ShouldBeNull();
        }

        [Fact]
        public async Task UpdateEducationInvalid_ValidationFailed()
        {
            var photo = new PhotoUploadResult { PublicId = "1000", Url = "photo-public-id" };
            _photoAccesor.Setup(pa => pa.AddPhoto(It.IsAny<IFormFile>())).ReturnsAsync(photo);

            var updateEducationDto = new UpdateEducationDto
            {
                Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7"),
                EducationInstitution = null,
                StartYear = DateTime.Now.AddYears(-2),
                GraduationYear = DateTime.Now.AddYears(-1),
                FieldOfStudy = null,
                Degree = "Master's",
                DoctorId = Guid.NewGuid(),
                EducationInstitutionLogoPhotoId = "Updated Campus"
            };


            var education = new Education { Id = updateEducationDto.Id };
            _mapper.Setup(x => x.Map<Education>(updateEducationDto)).Returns(education);

            // Act
            var result = await _handler.Handle(new UpdateEducationCommand{
                updateEducationDto = updateEducationDto
            }, CancellationToken.None);
            
            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Value.ShouldBeNull();
        }
    }
}
