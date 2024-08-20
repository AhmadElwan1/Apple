using Domain.DTOs.Employee;
using FluentValidation;

namespace Domain.Validators.EmployeeValidators
{
    public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(1, 25).WithMessage("Name must be between 1 and 25 characters.");

            RuleFor(x => x.YearsOfService)
                .GreaterThanOrEqualTo(0).WithMessage("YearsOfService must be a non-negative number.");

            RuleFor(x => x.UnitId)
                .GreaterThan(0).WithMessage("UnitId must be a positive integer.");

            RuleFor(x => x.CountryId)
                .GreaterThan(0).WithMessage("CountryId must be a positive integer.");

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Gender is not valid.");

            RuleFor(x => x.MaritalStatus)
                .IsInEnum().WithMessage("MaritalStatus is not valid.");

            RuleFor(x => x.Religion)
                .IsInEnum().WithMessage("Religion is not valid.");

            RuleFor(x => x.HasNewBorn)
                .NotNull().WithMessage("HasNewBorn must be specified.");
        }
    }
}