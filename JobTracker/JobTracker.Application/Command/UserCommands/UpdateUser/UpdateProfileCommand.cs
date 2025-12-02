using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace JobTracker.Application.Command.UserCommands.UpdateUser
{
    public class UpdateProfileCommand : IRequest<bool>
    {
        public string Phone { get; set; } = string.Empty;
    }
}
