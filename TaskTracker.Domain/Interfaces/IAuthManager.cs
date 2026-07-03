using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Domain.DTOs.Auth;

namespace TaskTracker.Domain.Interfaces
{
    public interface IAuthManager
    {
        Task RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RefreshTokenAsync(TokenRefreshRequest request);
        Task LogoutAsync(string refreshToken);
        Task RevokeTokenAsync(string refreshToken);
    }
}
