using Domain.DTOs.LeaveType;
using FluentValidation;

namespace Domain.Validators.LeaveRuleValidators
{
    public class LeaveTypeDtoValidator : AbstractValidator<LeaveTypeDto>
    {
        public LeaveTypeDtoValidator()
        {
            RuleFor(x => x.LeaveTypeName)
                .NotEmpty().WithMessage("LeaveTypeName is required.")
                .Length(1, 100).WithMessage("LeaveTypeName must be between 1 and 100 characters.");

            RuleFor(x => x.Entilement)
                .NotEmpty().WithMessage("Entilement is required.")
                .Length(1, 100).WithMessage("Entilement must be between 1 and 100 characters.");

            RuleFor(x => x.Accural)
                .NotEmpty().WithMessage("Accural is required.")
                .Length(1, 100).WithMessage("Accural must be between 1 and 100 characters.");

            RuleFor(x => x.CarryOver)
                .NotEmpty().WithMessage("CarryOver is required.")
                .Length(1, 100).WithMessage("CarryOver must be between 1 and 100 characters.");

            RuleFor(x => x.Expression)
                .NotEmpty().WithMessage("Expression is required.")
                .Length(1, 500).WithMessage("Expression must be between 1 and 500 characters.");

            RuleFor(x => x.NoticePeriod)
                .NotEmpty().WithMessage("NoticePeriod is required.")
                .Length(1, 100).WithMessage("NoticePeriod must be between 1 and 100 characters.");

            RuleFor(x => x.CountryId1)
                .GreaterThan(0).WithMessage("CountryId must be a positive integer.");

            RuleFor(x => x.TenantId)
                .GreaterThan(0).WithMessage("TenantId must be a positive integer.");

            RuleFor(x => x)
                .NotNull().WithMessage("DocumentRequired cannot be null.");
        }
    }
}
