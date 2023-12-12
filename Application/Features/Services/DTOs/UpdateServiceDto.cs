using Application.Features.Common;

namespace Application.Features.Services.DTOs;

public class UpdateServiceDto : BaseDto, IServiceDto
{
    public string ServiceName { get; set; } 
    public string ServiceDescription { get; set; }
}
