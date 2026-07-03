using System;
using System.Collections.Generic;
using System.Text;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}
