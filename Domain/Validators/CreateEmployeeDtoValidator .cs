using Domain.DTOs;
using FluentValidation;

namespace Domain.Validators
{
    public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(1, 25).WithMessage("Name must be between 1 and 25 characters.");

            RuleFor(x => x.IsMarried)
                .NotNull().WithMessage("IsMarried must be specified.");

            RuleFor(x => x.HasNewBorn)
                .NotNull().WithMessage("HasNewBorn must be specified.");
        }
    }
}