namespace Application.Features.Specialities.DTOs
{
    public interface ISpecialityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

    }
}