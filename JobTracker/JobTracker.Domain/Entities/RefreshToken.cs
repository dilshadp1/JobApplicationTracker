using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; private set; }
        public string TokenHash { get; private set; }
        public int UserId { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public string? ReplacedByTokenHash { get; private set; } 
        public bool IsActive => RevokedAt == null && !(DateTime.UtcNow >= ExpiryDate);

        public User? User { get; private set; }

        private RefreshToken() { }

        public RefreshToken(string rawToken, int userId)
        {
            UserId = userId;
            ExpiryDate = DateTime.UtcNow.AddDays(7);
            CreatedAt = DateTime.UtcNow;
            TokenHash = ComputeHash(rawToken);
        }

        public static string ComputeHash(string rawToken)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(rawToken);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
        public void Revoke(string newReplacedByTokenHash)
        {
            RevokedAt = DateTime.UtcNow;
            ReplacedByTokenHash = newReplacedByTokenHash;
        }
    }
}
