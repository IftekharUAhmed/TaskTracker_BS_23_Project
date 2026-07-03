using System;
using System.Collections.Generic;
using System.Text;

namespace TaskTracker.Domain.DTOs.Auth
{
    public class RevokeTokenRequest
    {
        public string RefreshToken { get; set; } = null!;
    }
}