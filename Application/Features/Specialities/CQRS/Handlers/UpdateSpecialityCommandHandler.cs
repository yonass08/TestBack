using AutoMapper;
using Application.Contracts.Persistence;
using Application.Features.Specialities.CQRS.Commands;
using Application.Features.Specialities.DTOs.Validators;
using MediatR;
using Application.Responses;

namespace Application.Features.Specialities.CQRS.Handlers
{
    public class UpdateSpecialityCommandHandler : IRequestHandler<UpdateSpecialityCommand, Result<Unit?>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateSpecialityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Unit?>> Handle(UpdateSpecialityCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateSpecialityDtoValidator();
            var validationResult = await validator.ValidateAsync(request.SpecialityDto);

            if (!validationResult.IsValid)
                return Result<Unit?>.Failure(validationResult.Errors[0].ErrorMessage);
            var response = new Result<Unit?>();

            var Speciality = await _unitOfWork.SpecialityRepository.Get(request.SpecialityDto.Id);
            if (Speciality == null){
                response.IsSuccess = false;
                response.Value = null;
                response.Error = "Speciality Not Found.";
                return response;
            };

            _mapper.Map(request.SpecialityDto, Speciality);
            await _unitOfWork.SpecialityRepository.Update(Speciality);

            if (await _unitOfWork.Save() > 0){
                response.IsSuccess = true;
                response.Value = Unit.Value;
                response.Message = "Speciality Updated Successfully.";
            }
            else{
                response.IsSuccess = false;
                response.Value = null;
                response.Error = "Speciality Update Failed.";
            }
        return response;

        }
    }
}
