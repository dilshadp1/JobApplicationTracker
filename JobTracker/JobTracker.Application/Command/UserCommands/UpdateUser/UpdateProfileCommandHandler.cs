using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;

namespace JobTracker.Application.Command.UserCommands.UpdateUser
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, bool>
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly ICurrentUserService _currentUser;

        public UpdateProfileCommandHandler(IGenericRepository<User> userRepo, ICurrentUserService currentUser)
        {
            _userRepo = userRepo;
            _currentUser = currentUser;
        }
        public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            int userId = _currentUser.UserId;
            User? user = await _userRepo.GetByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            user.UpdateProfile(request.Phone);

            await _userRepo.UpdateAsync(user);
            return true;
        }
    }
}
