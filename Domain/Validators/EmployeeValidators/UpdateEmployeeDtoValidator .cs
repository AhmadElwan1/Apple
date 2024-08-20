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
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.YearsOfService)
                .GreaterThanOrEqualTo(0).WithMessage("YearsOfService must be a non-negative number.")
                .When(x => x.YearsOfService.HasValue);

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Gender is not valid.")
                .When(x => x.Gender.HasValue);

            RuleFor(x => x.MaritalStatus)
                .IsInEnum().WithMessage("MaritalStatus is not valid.")
                .When(x => x.MaritalStatus.HasValue);

            RuleFor(x => x.Religion)
                .IsInEnum().WithMessage("Religion is not valid.")
                .When(x => x.Religion.HasValue);

            RuleFor(x => x.HasNewBorn)
                .NotNull().WithMessage("HasNewBorn must be specified.")
                .When(x => x.HasNewBorn.HasValue);
        }
    }
}