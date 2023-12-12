using Application.Features.Common;

namespace Application.Features.Services.DTOs;

public class ServiceDto : BaseDto, IServiceDto
{
    public string ServiceName { get; set; } 
    public string ServiceDescription { get; set; }

}
