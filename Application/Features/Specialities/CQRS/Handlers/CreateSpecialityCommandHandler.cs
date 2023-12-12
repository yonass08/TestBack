
using AutoMapper;
using Application.Contracts.Persistence;
using Application.Features.Specialities.CQRS.Commands;
using Application.Features.Specialities.DTOs.Validators;
using Application.Responses;
using MediatR;
using Domain;
using Application.Features.Specialities.DTOs;

namespace Application.Features.Specialities.CQRS.Handlers
{
    public class CreateSpecialityCommandHandler : IRequestHandler<CreateSpecialityCommand, Result<CreateSpecialityDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public CreateSpecialityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<Result<CreateSpecialityDto>> Handle(CreateSpecialityCommand request, CancellationToken cancellationToken)
        {

            var validator = new CreateSpecialityDtoValidator();
            var validationResult = await validator.ValidateAsync(request.SpecialityDto);

            if (!validationResult.IsValid)
                return Result<CreateSpecialityDto>.Failure(validationResult.Errors[0].ErrorMessage);
            

            var speciality = _mapper.Map<Speciality>(request.SpecialityDto);
            speciality.Id = Guid.NewGuid();
            var spec =  await _unitOfWork.SpecialityRepository.Add(speciality);

            var response = new Result<CreateSpecialityDto>();
            if (await _unitOfWork.Save() > 0)
            {
                response.Value = _mapper.Map<CreateSpecialityDto>(spec);
                response.IsSuccess = true;
                response.Message = "Create Speciality Successful.";
            }
            else
            {
                response.Value = null;
                response.IsSuccess = false;
                response.Error = "Create Speciality Failed.";
               
            }
            return response;
        }
    }
}
