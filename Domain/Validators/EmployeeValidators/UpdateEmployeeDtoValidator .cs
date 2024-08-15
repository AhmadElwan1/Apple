using Domain.DTOs.Employee;
using FluentValidation;

namespace Domain.Validators.EmployeeValidators
{
    public class UpdateEmployeeDtoValidator : AbstractValidator<UpdateEmployeeDto>
    {
        public UpdateEmployeeDtoValidator()
        {
            RuleFor(x => x.Name)
                .Length(1, 30).WithMessage("Name must be between 1 and 30 characters.")
                .When(x => x.Name != null);

            RuleFor(x => x.IsMarried)
                .NotNull().WithMessage("IsMarried must be specified.")
                .When(x => x.IsMarried.HasValue);

            RuleFor(x => x.HasNewBorn)
                .NotNull().WithMessage("HasNewBorn must be specified.")
                .When(x => x.HasNewBorn.HasValue);
        }
    }
}