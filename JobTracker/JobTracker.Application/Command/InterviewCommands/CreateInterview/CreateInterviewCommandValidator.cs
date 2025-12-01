using FluentValidation;

namespace JobTracker.Application.Command.InterviewCommands.CreateInterview
{
    public class CreateInterviewCommandValidator : AbstractValidator<CreateInterviewCommand>
    {
        public CreateInterviewCommandValidator()
        {
            RuleFor(x => x.ApplicationId)
                .GreaterThan(0);

            RuleFor(x => x.RoundName)
                .NotEmpty().WithMessage("Round name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Mode)
                .IsInEnum();

            RuleFor(x => x.Status)
                .IsInEnum();

            RuleFor(x => x.LocationUrl)
                .MaximumLength(500);
        }
    }
}
