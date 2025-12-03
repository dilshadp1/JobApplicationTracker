using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace JobTracker.Application.Command.AuthenticationCommands.LogoutCommand
{
    public class LogoutCommand : IRequest<bool>
    {
        public string Token { get; set; } = string.Empty;
    }
}
