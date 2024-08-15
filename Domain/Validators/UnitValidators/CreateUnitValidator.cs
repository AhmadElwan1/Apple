using Domain.DTOs.Unit;
using FluentValidation;

namespace Domain.Validators.UnitValidators;

public class CreateUnitDtoValidator : AbstractValidator<CreateUnitDto>
{
    public CreateUnitDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Unit name is required.")
            .MaximumLength(30).WithMessage("Unit name must not exceed 30 characters.");

        RuleFor(x => x.TenantId)
            .GreaterThan(0).WithMessage("Tenant ID must be greater than zero.");
    }
}