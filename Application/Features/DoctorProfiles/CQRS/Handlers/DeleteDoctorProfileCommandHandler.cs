
using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Features.DoctorProfiles.CQRS.Commands;
using Application.Responses;
using AutoMapper;
using MediatR;

namespace Application.Features.DoctorProfiles.CQRS.Handlers
{
    public class DeleteDoctorProfileCommandHandler : IRequestHandler<DeleteDoctorProfileCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DeleteDoctorProfileCommandHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper =mapper;
        }

        public async Task<Result<Unit>> Handle(DeleteDoctorProfileCommand request, CancellationToken cancellationToken)
        {
            var response = new Result<Unit>();
            var doctorProfile = await _unitOfWork.DoctorProfileRepository.GetDoctorProfileDetail(request.Id);
            if (doctorProfile == null)
            {

                response.IsSuccess = false;
                response.Error = $"{new NotFoundException(nameof(doctorProfile), request.Id)}";
                return response;

            }
            await _unitOfWork.DoctorProfileRepository.Delete(doctorProfile);
            if (await _unitOfWork.Save() == 0)
            {
                response.IsSuccess = false;
                response.Error = "server error";
                return response;
            }
            else
            {
                response.IsSuccess = true;
                return response;
            }

        }

    }
}