using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Interfaces;
using TaskTracker.Domain.Managers;
using Xunit;

namespace TaskTracker.Tests.Managers
{
    public class AuthManagerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<IRefreshTokenRepository> _mockRefreshTokenRepo;
        private readonly Mock<IPasswordHasher> _mockPasswordHasher;
        private readonly Mock<IJwtTokenGenerator> _mockJwtGenerator;
        private readonly Mock<IUnitOfWork> _mockUoW;
        private readonly AuthManager _authManager;

        public AuthManagerTests()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _mockRefreshTokenRepo = new Mock<IRefreshTokenRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _mockJwtGenerator = new Mock<IJwtTokenGenerator>();
            _mockUoW = new Mock<IUnitOfWork>();
            _authManager = new AuthManager(
                _mockUserRepo.Object,
                _mockRefreshTokenRepo.Object,
                _mockPasswordHasher.Object,
                _mockJwtGenerator.Object,
                _mockUoW.Object);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowForbiddenException_WhenUserNotFound()
        {
            //arrange
            var request = new LoginRequest { Username = "wronguser", Password = "password123" };

            _mockUserRepo.Setup(repo => repo.GetByUsernameAsync(request.Username))
                .ReturnsAsync((User?)null);

            //act
            Func<Task> act = async () => await _authManager.LoginAsync(request);

            //assert  matches actual code throwing forbiddenexception
            await act.Should().ThrowAsync<TaskTracker.Domain.Exceptions.ForbiddenException>();
        }
    }
}