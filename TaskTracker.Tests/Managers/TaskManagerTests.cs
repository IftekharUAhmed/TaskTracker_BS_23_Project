using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using TaskTracker.Domain.DTOs.Tasks;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Exceptions;
using TaskTracker.Domain.Interfaces;
using TaskTracker.Domain.Managers;
using Xunit;

namespace TaskTracker.Tests.Managers
{
    public class TaskManagerTests
    {
        private readonly Mock<ITaskRepository> _mockTaskRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IUnitOfWork> _mockUoW;
        private readonly TaskManager _taskManager;

        public TaskManagerTests()
        {
            _mockTaskRepo = new Mock<ITaskRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockUoW = new Mock<IUnitOfWork>();

            _taskManager = new TaskManager(
                _mockTaskRepo.Object,
                _mockMapper.Object,
                _mockUoW.Object);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldReturnTask_WhenTaskExists()
        {
            //arrange
            int taskId = 1;
            var taskItem = new TaskItem { Id = taskId, Title = "Test Task" };
            var expectedResponse = new TaskWithCommentsResponse { Id = taskId, Title = "Test Task" };

            _mockTaskRepo.Setup(repo => repo.GetWithCommentsAsync(taskId))
                .ReturnsAsync(taskItem);

            _mockMapper.Setup(m => m.Map<TaskWithCommentsResponse>(taskItem))
                .Returns(expectedResponse);

            //act
            var result = await _taskManager.GetTaskByIdAsync(taskId);

            //assert
            result.Should().NotBeNull();
            result.Id.Should().Be(taskId);
            result.Title.Should().Be("Test Task");
        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldThrowNotFoundException_WhenTaskDoesNotExist()
        {
            //arrange
            int taskId = 99;
            _mockTaskRepo.Setup(repo => repo.GetWithCommentsAsync(taskId))
                .ReturnsAsync((TaskItem?)null);

            //act
            System.Func<Task> act = async () => await _taskManager.GetTaskByIdAsync(taskId);

            //assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Task with ID {taskId} not found.");
        }
    }
}