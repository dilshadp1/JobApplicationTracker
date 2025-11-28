using JobTracker.Application.DTO;
using MediatR;

namespace JobTracker.Application.Command.CreateRefreshToken
{
    public class CreateRefreshTokenCommand : IRequest<AuthResponse>
    {
        public string Token { get; set; } = string.Empty;
    }
}
