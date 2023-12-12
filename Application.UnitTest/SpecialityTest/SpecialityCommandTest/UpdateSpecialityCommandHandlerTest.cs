

using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.Persistence;
using Application.Features.Specialities.CQRS.Commands;
using Application.Features.Specialities.CQRS.Handlers;
using Application.Features.Specialities.DTOs;
using Application.Responses;
using Application.UnitTest.Mocks;
using AutoMapper;
using Domain;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using Shouldly;
using Xunit;

namespace Application.UnitTest.SpecialityTest.SpecialityCommandTest;
    public class UpdateSpecialityCommandHandlerTest
    {
        private Mock<IMapper> _mapper { get; set; }
        private readonly UpdateSpecialityCommandHandler _handler;
        private Mock<IUnitOfWork> _mockUnitOfWork { get; set; }


        public UpdateSpecialityCommandHandlerTest()
        {
            _mapper =new Mock<IMapper>();
            _mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();

            _handler = new UpdateSpecialityCommandHandler(_mockUnitOfWork.Object, _mapper.Object);
        }

        [Fact]
        public async Task UpdateSpecialityValid()
        {

            var updateSpecialityDto = new UpdateSpecialityDto
            {
                Id = Guid.NewGuid(),
               Name = "Updated",
               Description = "This is an updated description"

            };
            var speciality = new Speciality
            {
                Id = updateSpecialityDto.Id,
            };

            var expectedResult = Result<Unit?>.Success(Unit.Value);
            _mockUnitOfWork.Setup(u => u.SpecialityRepository.Get(updateSpecialityDto.Id)).ReturnsAsync(speciality);
            _mockUnitOfWork.Setup(u => u.SpecialityRepository.Update(It.IsAny<Speciality>()));
            _mockUnitOfWork.Setup(u => u.Save()).ReturnsAsync(1);
            _mapper.Setup(m => m.Map(updateSpecialityDto, speciality)).Returns(speciality);

           var result = await _handler.Handle(new UpdateSpecialityCommand() { SpecialityDto = updateSpecialityDto }, CancellationToken.None);

        result.IsSuccess.ShouldBe(expectedResult.IsSuccess);
        _mockUnitOfWork.Verify(u => u.SpecialityRepository.Get(updateSpecialityDto.Id), Times.Once);
        _mockUnitOfWork.Verify(u => u.SpecialityRepository.Update(It.IsAny<Speciality>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
            _mapper.Verify(m => m.Map(updateSpecialityDto, speciality), Times.Once);



    }

    [Fact]
        public async Task UpdateSpecialityInvalid_SpecialityNotFound()
        {

        var updateSpecialityDto = new UpdateSpecialityDto
        {
            Id = Guid.NewGuid(),
            Name = "Oncology",
            Description = "Sample Description"
        };
        var expectedResult = Result<Unit>.Failure("Speciality Not Found.");
        _mockUnitOfWork.Setup(u => u.SpecialityRepository.Get(updateSpecialityDto.Id)).ReturnsAsync((Speciality)null);

        var result = await _handler.Handle(new UpdateSpecialityCommand
        {
            SpecialityDto = updateSpecialityDto
        }, CancellationToken.None);

        result.IsSuccess.ShouldBe(expectedResult.IsSuccess);
        result.Error.ShouldBe(expectedResult.Error);
        _mockUnitOfWork.Verify(u => u.SpecialityRepository.Get(updateSpecialityDto.Id), Times.Once);
        _mockUnitOfWork.Verify(u => u.SpecialityRepository.Update(It.IsAny<Speciality>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.Save(), Times.Never);
        _mapper.Verify(m => m.Map(updateSpecialityDto, It.IsAny<Speciality>()), Times.Never);


    }

    [Fact]
    public async Task UpdateSpecialityInvalid_ValidationFailed()
    {
        var updateSpecialityDto = new UpdateSpecialityDto
        {
            Id = Guid.NewGuid(),
        };



        // Set up the validation failure
        var validationFailure = new ValidationFailure("Name", "Name is required.");
        var validationErrors = new ValidationFailure[] { validationFailure };
        var validationResult = new ValidationResult(validationErrors);

        // Set up the expected result
        var expectedResult = Result<Unit>.Failure(validationFailure.ErrorMessage);


    var validatorMock = new Mock<IValidator<UpdateSpecialityDto>>();
    validatorMock.Setup(v => v.ValidateAsync(updateSpecialityDto, CancellationToken.None)).ReturnsAsync(validationResult);


    // Act
    var result = await _handler.Handle(new UpdateSpecialityCommand{SpecialityDto = updateSpecialityDto }, CancellationToken.None);

    // Assert
    result.IsSuccess.ShouldBe(expectedResult.IsSuccess);
            result.Error.ShouldBe(expectedResult.Error);
            _mockUnitOfWork.Verify(u => u.SpecialityRepository.Get(updateSpecialityDto.Id), Times.Never);
            _mockUnitOfWork.Verify(u => u.SpecialityRepository.Update(It.IsAny<Speciality>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.Save(), Times.Never);
            _mapper.Verify(m => m.Map(updateSpecialityDto, It.IsAny<Speciality>()), Times.Never);
        }
}