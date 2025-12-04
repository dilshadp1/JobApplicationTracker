using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JobTracker.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace JobTracker.Infrastructure.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        public int UserId => GetUserId();

        private int GetUserId()
        {
            ClaimsPrincipal? user = httpContextAccessor.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated != true)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            Claim? idClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            idClaim ??= user.FindFirst("sub");

            if (idClaim != null && int.TryParse(idClaim.Value, out int userId))
            {
                return userId;
            }

            throw new UnauthorizedAccessException("User ID claim is missing or invalid.");
        }
    }
}
