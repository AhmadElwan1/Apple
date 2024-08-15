using Domain.DTOs.Unit;
using FluentValidation;

namespace Domain.Validators.UnitValidators;

public class UpdateUnitDtoValidator : AbstractValidator<UpdateUnitDto>
{
    public UpdateUnitDtoValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(30).WithMessage("Unit name must not exceed 30 characters.")
            .When(x => !string.IsNullOrEmpty(x.Name));
    }
}