using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using JobTracker.Application.DTO;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;

namespace JobTracker.Application.Command.RefreshToken
{
    public class RefreshTokenCommandHandler(
     IGenericRepository<User> userRepository,
     IGenericRepository<Domain.Entities.RefreshToken> refreshRepo,
     IJwtTokenGenerator tokenGenerator)
     : IRequestHandler<RefreshTokenCommand, AuthResponse>
    {
        public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            string incomingTokenHash = Domain.Entities.RefreshToken.ComputeHash(request.Token);

            var storedToken = await refreshRepo.GetFirstOrDefaultAsync(t => t.TokenHash == incomingTokenHash);

            if (storedToken == null)
                throw new Exception("Invalid refresh token.");

            if (storedToken.RevokedAt != null)
            {
                throw new Exception("Security Alert: Use of revoked token detected.");
            }

            if (storedToken.ExpiryDate < DateTime.UtcNow)
                throw new Exception("Expired refresh token.");

            var user = await userRepository.GetByIdAsync(storedToken.UserId);
            if (user == null) throw new Exception("User not found.");

            string newAccessToken = tokenGenerator.GenerateToken(user);
            string newRefreshTokenRaw = tokenGenerator.GenerateRefreshToken();

            string newRefreshTokenHash = Domain.Entities.RefreshToken.ComputeHash(newRefreshTokenRaw);

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    storedToken.Revoke(newRefreshTokenHash);
                    await refreshRepo.UpdateAsync(storedToken);

                    var newEntity = new Domain.Entities.RefreshToken(newRefreshTokenRaw, user.Id);
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
