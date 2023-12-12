using Application.Contracts.Persistence;
using Application.Features.Specialities.CQRS.Commands;
using Application.Responses;
using MediatR;

namespace Application.Features.Specialities.CQRS.Handlers
{
    public class DeleteSpecialityCommandHandler : IRequestHandler<DeleteSpecialityCommand, Result<Guid?>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSpecialityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid?>> Handle(DeleteSpecialityCommand request, CancellationToken cancellationToken)
        {

            var speciality = await _unitOfWork.SpecialityRepository.Get(request.Id);
            var response = new Result<Guid?>();
            if (speciality is null) {
                response.IsSuccess = false;
                response.Value = null;
                response.Error = "Speciality Not Found.";
                return response;
            };

            await _unitOfWork.SpecialityRepository.Delete(speciality);
           
            if (await _unitOfWork.Save() > 0)
                {
                    response.IsSuccess = true;
                    response.Value = speciality.Id;
                    response.Message = "Speciality Deleted Successfully.";

                }else{
                response.IsSuccess = false;
                response.Value = null;
                response.Error = "Speciality Deleted Failed.";
                }

            return response;

        }
    }
}
