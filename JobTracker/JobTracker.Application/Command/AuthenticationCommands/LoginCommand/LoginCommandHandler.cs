using JobTracker.Application.DTO;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;

namespace JobTracker.Application.Command.AuthenticationCommands.LoginCommand
{
    public class LoginUserCommandHandler(IGenericRepository<User> userRepository, IGenericRepository<RefreshToken> refreshRepo, IJwtTokenGenerator tokenGenerator) : IRequestHandler<LoginCommand, AuthResponse>
    {
        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            User? user = (await userRepository.GetAsync(u => u.Email == request.Email)).FirstOrDefault();

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }


            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            string accessToken = tokenGenerator.GenerateToken(user);
            string refreshTokenString = tokenGenerator.GenerateRefreshToken();

            RefreshToken refreshTokenEntity = new RefreshToken(refreshTokenString, user.Id);
            await refreshRepo.AddAsync(refreshTokenEntity);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenString
            };
        }
    }
}
