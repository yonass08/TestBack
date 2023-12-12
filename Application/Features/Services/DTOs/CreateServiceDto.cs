namespace Application.Features.Services.DTOs;

public class CreateServiceDto : IServiceDto
{
    public string ServiceName { get; set; } 
    public string ServiceDescription { get; set; }
}
