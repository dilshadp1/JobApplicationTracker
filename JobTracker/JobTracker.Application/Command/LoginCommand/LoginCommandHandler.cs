using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;

namespace JobTracker.Application.Command.LoginCommand
{
    public class LoginUserCommandHandler(IGenericRepository<User> userRepository, IJwtTokenGenerator tokenGenerator) : IRequestHandler<LoginCommand, string>
    {
        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var users = await userRepository.GetAsync(u => u.Email == request.Email);
            var user = users.FirstOrDefault();

            if (user == null)
            {
                throw new Exception("Invalid email");
            }


            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                throw new Exception("Incorrect password.");
            }

            string token = tokenGenerator.GenerateToken(user);

            return token;
        }
    }
}
