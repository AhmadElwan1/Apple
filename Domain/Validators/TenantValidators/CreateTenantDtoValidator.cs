using Domain.DTOs.Tenant;
using FluentValidation;

namespace Domain.Validators.TenantValidators
{
    public class CreateTenantDtoValidator : AbstractValidator<CreateTenantDto>
    {
        public CreateTenantDtoValidator()
        {
            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(30).WithMessage("Name must not exceed 30 characters.");
        }
    }
}