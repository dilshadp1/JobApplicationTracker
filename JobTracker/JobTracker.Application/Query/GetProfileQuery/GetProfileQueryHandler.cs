using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobTracker.Application.DTO;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;

namespace JobTracker.Application.Query.GetProfileQuery
{
    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, ProfileDto>
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly ICurrentUserService _currentUser;

        public GetProfileQueryHandler(IGenericRepository<User> userRepo, ICurrentUserService currentUser)
        {
            _userRepo = userRepo;
            _currentUser = currentUser;
        }
        public async Task<ProfileDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            int userId = _currentUser.UserId;
            User? user = await _userRepo.GetByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return new ProfileDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone ?? string.Empty
            };
        }
    }
}
