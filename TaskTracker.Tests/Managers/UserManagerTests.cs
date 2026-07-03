using AutoMapper;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using TaskTracker.Domain.DTOs.Users;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Exceptions;
using TaskTracker.Domain.Interfaces;
using TaskTracker.Domain.Managers;
using Xunit;

namespace TaskTracker.Tests.Managers
{
    public class UserManagerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IUnitOfWork> _mockUoW;
        private readonly UserManager _userManager;

        public UserManagerTests()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockUoW = new Mock<IUnitOfWork>();

            _userManager = new UserManager(
                _mockUserRepo.Object,
                _mockMapper.Object,
                _mockUoW.Object);
        }

        [Fact]
        public async Task ChangeUserRoleAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            //arrange
            int userId = 99;
            var request = new UpdateUserRoleRequest { Role = "Admin" };

            _mockUserRepo.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync((User?)null);

            //act
            Func<Task> act = async () => await _userManager.ChangeUserRoleAsync(userId, request);

            //assert
            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}