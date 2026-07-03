using System;
using System.Collections.Generic;
using System.Text;

namespace TaskTracker.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string? ReplacedByToken { get; set; }

        //navigation property
        public User User { get; set; } = null!;
    }
}