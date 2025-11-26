using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobTracker.Domain.Enums;


namespace JobTracker.Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public UserRole Role { get; private set; }
        public string Phone { get; private set; }
        public DateTime CreatedAt { get; private set; }

        //public ICollection<JobApplication> JobApplications { get; private set; } = new List<JobApplication>();

        private User() { }

        public User(string firstName, string lastName, string email, string passwordHash, string phone,UserRole role = UserRole.User)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            Phone = phone;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
