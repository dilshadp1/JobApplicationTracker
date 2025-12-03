using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;

namespace JobTracker.Application.Command.AuthenticationCommands.LogoutCommand
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
    {
        private readonly IGenericRepository<RefreshToken> _refreshRepo;

        public LogoutCommandHandler(IGenericRepository<RefreshToken> refreshRepo)
        {
            _refreshRepo = refreshRepo;
        }

        public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            string tokenHash = RefreshToken.ComputeHash(request.Token);

            RefreshToken? storedToken = await _refreshRepo.GetFirstOrDefaultAsync(t => t.TokenHash == tokenHash);

            if (storedToken != null)
            {
                storedToken.Revoke(newReplacedByTokenHash: null);
                await _refreshRepo.UpdateAsync(storedToken);
            }

            return true;
        }
    }
}
