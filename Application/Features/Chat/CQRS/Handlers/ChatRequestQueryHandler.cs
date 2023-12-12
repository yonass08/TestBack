using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Application.Features.Chat.CQRS.Queries;
using Application.Features.Chat.DTOs;
using Application.Features.DoctorProfiles.DTOs;
using Application.Features.InstitutionProfiles.DTOs;
using Application.Responses;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.Chat.CQRS.Handlers;

public class ChatRequestQueryHandler : IRequestHandler<ChatRequestQuery, Result<ChatResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IChatRequestSender _chatRequestSender;

    public ChatRequestQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IChatRequestSender chatRequestSender)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _chatRequestSender = chatRequestSender;
    }

    public async Task<Result<ChatResponseDto>> Handle(ChatRequestQuery request, CancellationToken cancellationToken)
    {

        // create the response
        var response = new Result<ChatResponseDto>();

        // send request to chatbot api and get api response
        var ApiResponse = await _chatRequestSender.SendMessage(request.ChatRequestDto);

        // if error occured
        if (ApiResponse.Error != null)
        {
            response.Error = ApiResponse.Error.message;
            response.IsSuccess = false;
            return response;
        }

        // if data reply message
        response.IsSuccess = true;

        var chatResponse = new ChatResponseDto
        {
            reply = ApiResponse.Data.message
        };

        var specialization = ApiResponse.Data.specialization;

        if (!string.IsNullOrEmpty(specialization))
        {

            chatResponse.Speciality = specialization;

            var Doctors = await _unitOfWork.DoctorProfileRepository.FilterDoctors(Guid.Empty, null, new List<string?> { specialization }, -1, null);
            var Institutions = Doctors.Select(d => d.MainInstitution).Distinct();
            var DetailInstitution = new List<InstitutionProfileDetailDto>();

            foreach (var institution in Institutions)
            {

                var inst = await _unitOfWork.InstitutionProfileRepository.GetPopulatedInstitution(institution.Id);
                var mapped_inst = _mapper.Map<InstitutionProfileDetailDto>(inst);
                mapped_inst.Doctors = mapped_inst.Doctors.Where(d => d.Specialities.Contains(specialization)).ToList();

                DetailInstitution.Add(mapped_inst);

            }
            chatResponse.Institutions = DetailInstitution;
        }

        response.Value = chatResponse;
        return response;

    }

}