using FluentValidation;

namespace JobTracker.Application.Command.CreateJobApplication
{
    public class CreateJobApplicationCommandValidator : AbstractValidator<CreateJobApplicationCommand>
    {
        public CreateJobApplicationCommandValidator()
        {
            
            RuleFor(x => x.Company)
                .NotEmpty().WithMessage("Company name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Position)
                .NotEmpty().WithMessage("Position is required.")
                .MaximumLength(100);

            RuleFor(x => x.AppliedDate)
                .LessThanOrEqualTo(DateTime.UtcNow);

            RuleFor(x => x.Status)
                .IsInEnum();

            RuleFor(x => x.SalaryExpectation)
                .GreaterThan(0);

            RuleFor(x => x.JobUrl)
                .MaximumLength(500);
        }
    }
}
