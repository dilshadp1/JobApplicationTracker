using FluentValidation;

namespace JobTracker.Application.Command.UserCommands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            // 1. First Name Validation
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .MinimumLength(3).WithMessage("First Name must be at least 3 characters.")
                .MaximumLength(50).WithMessage("First Name must not exceed 50 characters.");

            // 2. Last Name Validation
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.");

            // 3. Email Validation
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            // 4. Password Validation
            RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
            .Matches(@"[\W]").WithMessage("Password must contain at least one special character (e.g. ! @ # $).");

            // 5. Phone Validation (Strict)
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\d+$").WithMessage("Phone number must contain only digits.")
                .Length(10).WithMessage("Phone number must be exactly 10 digits.");
        }
    }
}
