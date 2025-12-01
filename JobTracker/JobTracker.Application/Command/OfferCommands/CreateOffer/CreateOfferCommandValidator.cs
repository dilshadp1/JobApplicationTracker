using FluentValidation;


namespace JobTracker.Application.Command.OfferCommands.CreateOffer
{
    public class CreateOfferCommandValidator : AbstractValidator<CreateOfferCommand>
    {
        public CreateOfferCommandValidator()
        {
            RuleFor(x => x.ApplicationId)
                .GreaterThan(0);

            RuleFor(x => x.Salary)
                .GreaterThan(0);

            RuleFor(x => x.OfferDate)
                .LessThanOrEqualTo(DateTime.UtcNow);

            RuleFor(x => x.Deadline)
                .GreaterThanOrEqualTo(x => x.OfferDate);
        }
    }
}
