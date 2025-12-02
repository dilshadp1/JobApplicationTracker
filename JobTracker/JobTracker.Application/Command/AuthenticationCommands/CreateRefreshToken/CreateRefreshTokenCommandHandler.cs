using System.Transactions;
using JobTracker.Application.DTO;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;

namespace JobTracker.Application.Command.AuthenticationCommands.CreateRefreshToken
{
    public class CreateRefreshTokenCommandHandler(

         IGenericRepository<User> userRepository,
         IGenericRepository<RefreshToken> refreshRepo,
         IJwtTokenGenerator tokenGenerator
        ) : IRequestHandler<CreateRefreshTokenCommand, AuthResponse>
    {
        public async Task<AuthResponse> Handle(CreateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            string incomingTokenHash = RefreshToken.ComputeHash(request.Token);
            RefreshToken storedToken = await refreshRepo.GetFirstOrDefaultAsync(t => t.TokenHash == incomingTokenHash);

            if (storedToken == null)
                throw new UnauthorizedAccessException("Invalid refresh token.");

            if (storedToken.RevokedAt != null)
                throw new UnauthorizedAccessException("Security Alert: Use of revoked token detected.");

            if (storedToken.ExpiryDate < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Expired refresh token.");

            var user = await userRepository.GetByIdAsync(storedToken.UserId); //User?

            if (user == null)
                throw new UnauthorizedAccessException("User not found.");

            string newAccessToken = tokenGenerator.GenerateToken(user);
            string newRefreshTokenRaw = tokenGenerator.GenerateRefreshToken();
            string newRefreshTokenHash = RefreshToken.ComputeHash(newRefreshTokenRaw);

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))//TransactionScope
            {
                try
                {
                    storedToken.Revoke(newRefreshTokenHash);
                    await refreshRepo.UpdateAsync(storedToken);

                    var newEntity = new RefreshToken(newRefreshTokenRaw, user.Id);//refreshtoken
                    await refreshRepo.AddAsync(newEntity);

                    scope.Complete();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return new AuthResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshTokenRaw
            };
        }
    }
}
