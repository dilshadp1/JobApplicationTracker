using JobTracker.Domain.Entities;

namespace JobTracker.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();
    }
}
