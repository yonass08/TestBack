namespace Application.Features.Specialities.DTOs
{
    public class CreateSpecialityDto : ISpecialityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }


    }
}