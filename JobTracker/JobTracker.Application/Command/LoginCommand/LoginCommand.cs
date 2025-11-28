using JobTracker.Application.DTO;
using MediatR;

namespace JobTracker.Application.Command.LoginCommand
{
    public class LoginCommand : IRequest<AuthResponse>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
