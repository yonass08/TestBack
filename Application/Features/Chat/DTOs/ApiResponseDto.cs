using Application.Features.Chat.Models;

namespace Application.Features.Chat.DTOs;

public class ApiResponseDto
{
    public Data? Data {get; set;}

    public Error? Error {get; set;}
}
