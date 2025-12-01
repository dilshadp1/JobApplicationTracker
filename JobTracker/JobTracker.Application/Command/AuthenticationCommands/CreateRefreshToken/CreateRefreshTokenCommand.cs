using JobTracker.Application.DTO;
using MediatR;

namespace JobTracker.Application.Command.AuthenticationCommands.CreateRefreshToken
{
    public class CreateRefreshTokenCommand : IRequest<AuthResponse>
    {
        public string Token { get; set; } = string.Empty;
    }
}
