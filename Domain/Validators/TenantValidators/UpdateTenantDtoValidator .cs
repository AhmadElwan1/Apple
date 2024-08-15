using Domain.DTOs.Tenant;
using FluentValidation;

namespace Domain.Validators.TenantValidators;

public class UpdateTenantDtoValidator : AbstractValidator<UpdateTenantDto>
{
    public UpdateTenantDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(40).WithMessage("Name must not exceed 40 characters.");
    }
}