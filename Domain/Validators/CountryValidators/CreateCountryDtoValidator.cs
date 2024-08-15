using Domain.DTOs.Country;
using FluentValidation;

namespace Domain.Validators.CountryValidators;

public class CreateCountryDtoValidator : AbstractValidator<CreateCountryDto>
{

    public CreateCountryDtoValidator()
    {
        RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Name is required.")
        .Length(1, 25).WithMessage("Name must be between 1 and 25 characters.");
    }

}
