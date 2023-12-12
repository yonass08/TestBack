using Application.Features.Chat.Models;
using Application.Features.DoctorProfiles.DTOs;
using Application.Features.InstitutionProfiles.DTOs;
using Domain;

namespace Application.Features.Chat.DTOs;

public class ChatResponseDto
{
    public string reply {get; set;}
    
    public List<InstitutionProfileDetailDto>? Institutions {get; set;}

    public string Speciality { get; set; } =  "";
}
