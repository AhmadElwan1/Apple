using Domain.DTOs;
using FluentValidation;

namespace Domain.Validators
{
    public class LeaveRuleDtoValidator : AbstractValidator<LeaveRuleDto>
    {
        public LeaveRuleDtoValidator()
        {
            RuleFor(x => x.RuleName)
                .NotEmpty().WithMessage("RuleName is required.")
                .Length(1, 80).WithMessage("RuleName must be between 1 and 80 characters.");

            RuleFor(x => x.Expression)
                .NotEmpty().WithMessage("Expression is required.");

            RuleFor(x => x.SuccessEvent)
                .NotEmpty().WithMessage("SuccessEvent is required.");

            RuleFor(x => x.FailureEvent)
                .NotEmpty().WithMessage("FailureEvent is required.");

            RuleFor(x => x.CountryId)
                .GreaterThan(0).WithMessage("CountryId must be a positive integer.");
        }
    }
}