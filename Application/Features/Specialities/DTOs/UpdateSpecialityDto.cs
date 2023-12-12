using Application.Features.Common;

namespace Application.Features.Specialities.DTOs
{
    public class UpdateSpecialityDto : BaseDto, ISpecialityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}