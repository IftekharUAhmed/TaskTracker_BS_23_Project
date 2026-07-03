using System;
using System.Collections.Generic;
using System.Text;

namespace TaskTracker.Domain.DTOs.Auth
{
    public class TokenRefreshRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
