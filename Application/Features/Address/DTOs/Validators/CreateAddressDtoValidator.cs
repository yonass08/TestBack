using FluentValidation;

namespace Application.Features.Addresses.DTOs.Validators
{
    public class CreateAddressDtoValidator : AbstractValidator<CreateAddressDto>
    {
        public CreateAddressDtoValidator()
        {
            Include(new IAddressDtoValidator());
        }
    }
}