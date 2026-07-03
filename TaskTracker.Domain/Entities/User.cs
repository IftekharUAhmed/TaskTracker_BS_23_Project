using System;
using System.Collections.Generic;
using System.Text;


namespace TaskTracker.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        //navigation property
        public ICollection<RefreshToken> RefreshTokens { get; set; }

        public User()
        {
            RefreshTokens = new List<RefreshToken>();
        }
    }
}
