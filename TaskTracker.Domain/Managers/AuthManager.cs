using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Exceptions;
using TaskTracker.Domain.Interfaces;

namespace TaskTracker.Domain.Managers
{
    public class AuthManager : IAuthManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public AuthManager(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            //business Validation
            bool userExists = await _userRepository.ExistsAsync(request.Username, request.Email);
            if (userExists)
            {
                throw new Exception("Username or Email is already taken.");
            }
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                Role = UserRoles.Member,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RevokeTokenAsync(string refreshToken)
        {
            var tokenEntity = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (tokenEntity != null && tokenEntity.RevokedAt == null)
            {
                tokenEntity.RevokedAt = DateTime.UtcNow;
                _refreshTokenRepository.Update(tokenEntity);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new ForbiddenException("Invalid username or password.");
            }

            var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);
            var refreshTokenString = _jwtTokenGenerator.GenerateRefreshToken();

            var refreshToken = new RefreshToken
            {
                Token = refreshTokenString,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7), 
            };

            await _refreshTokenRepository.AddAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenString
            };
        }

        public async Task<AuthResponse> RefreshTokenAsync(TokenRefreshRequest request)
        {
            var existingToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);

            if (existingToken == null)
            {
                throw new ForbiddenException("Invalid refresh token.");
            }

            //token theft detection
            if (existingToken.RevokedAt != null)
            {
                await _refreshTokenRepository.RevokeAllTokensForUserAsync(existingToken.UserId);
                await _unitOfWork.SaveChangesAsync();
                throw new ForbiddenException("Attempted reuse of revoked token. All tokens revoked.");
            }

            if (existingToken.ExpiresAt < DateTime.UtcNow)
            {
                throw new ForbiddenException("Refresh token has expired.");
            }

            var user = await _userRepository.GetByIdAsync(existingToken.UserId);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            //rotate token
            var newAccessToken = _jwtTokenGenerator.GenerateAccessToken(user);
            var newRefreshTokenString = _jwtTokenGenerator.GenerateRefreshToken();

            existingToken.RevokedAt = DateTime.UtcNow;
            existingToken.ReplacedByToken = newRefreshTokenString;
            _refreshTokenRepository.Update(existingToken);

            var newRefreshToken = new RefreshToken
            {
                Token = newRefreshTokenString,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _refreshTokenRepository.AddAsync(newRefreshToken);
            await _unitOfWork.SaveChangesAsync();

            return new AuthResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshTokenString
            };
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var tokenEntity = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (tokenEntity != null && tokenEntity.RevokedAt == null)
            {
                tokenEntity.RevokedAt = DateTime.UtcNow;
                _refreshTokenRepository.Update(tokenEntity);
                await _unitOfWork.SaveChangesAsync();

            }
        }
    }
}
