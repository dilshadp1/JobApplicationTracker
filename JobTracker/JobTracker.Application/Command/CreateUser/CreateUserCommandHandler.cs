using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Enums;
using MediatR;

namespace JobTracker.Application.Command.CreateUser
{
    public class CreateUserCommandHandler(IGenericRepository<User> userRepository) : IRequestHandler<CreateUserCommand, int>
    {
        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            bool userExists = await userRepository.AnyAsync(u => u.Email == request.Email);
            if (userExists)
            {
                throw new Exception("User with this email already exists.");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            User newUser = new User(request.FirstName, request.LastName, request.Email, passwordHash, request.Phone, UserRole.User);

            await userRepository.AddAsync(newUser);

            return newUser.Id;
        }
    }
}
