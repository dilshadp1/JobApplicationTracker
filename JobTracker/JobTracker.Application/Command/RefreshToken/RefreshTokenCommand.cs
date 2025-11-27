using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobTracker.Application.DTO;
using MediatR;

namespace JobTracker.Application.Command.RefreshToken
{
    public class RefreshTokenCommand : IRequest<AuthResponse>
    {
        public string Token { get; set; } = string.Empty;
    }
}
