using AutoMapper;
using Application.Contracts.Persistence;
using MediatR;
using Application.Responses;
using Application.Features.InstitutionProfiles.CQRS.Queries;
using Application.Features.InstitutionProfiles.DTOs;

namespace Application.Features.InstitutionProfiles.CQRS.Handlers
{
    public class GetInstitutionProfileDetailQueryHandler : IRequestHandler<GetInstitutionProfileDetailQuery, Result<InstitutionProfileDetailDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetInstitutionProfileDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<InstitutionProfileDetailDto>> Handle(GetInstitutionProfileDetailQuery request, CancellationToken cancellationToken)
        {
            var institutionProfile = await _unitOfWork.InstitutionProfileRepository.GetPopulatedInstitution(request.Id);
            if (institutionProfile == null)
                return Result<InstitutionProfileDetailDto>.Failure(error: "Item not found.");

            
            var institutionProfileDto = _mapper.Map<InstitutionProfileDetailDto>(institutionProfile);
            var allEducations = await _unitOfWork.EducationRepository.GetAllPopulated();
            var allSpecialities = await _unitOfWork.SpecialityRepository.GetAll();
            ICollection<string> specialtyNames = allSpecialities.Select(s => s.Name).ToList();
            ICollection<EducationalInstitutionDto> educationDtos = _mapper.Map<ICollection<EducationalInstitutionDto>>(allEducations);
            ICollection<EducationalInstitutionDto> uniqueEducationDtos = educationDtos
                .GroupBy(dto => dto.InstitutionName)
                .Select(group => group.First())
                .ToList();

            institutionProfileDto.AllEducationalInstitutions = uniqueEducationDtos;
            institutionProfileDto.AllSpecialities = specialtyNames;

            return Result<InstitutionProfileDetailDto>.Success(institutionProfileDto);
        }

    }
}