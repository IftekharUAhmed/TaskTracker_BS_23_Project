using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Data;
using TaskTracker.Infrastructure.Repositories;
using Xunit;

namespace TaskTracker.Tests.Repositories
{
    public class TaskRepositoryTests : IDisposable
    {
        private readonly TaskTrackerDbContext _context;
        private readonly TaskRepository _repository;

        public TaskRepositoryTests()
        {
            //for isolated testing
            var options = new DbContextOptionsBuilder<TaskTrackerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TaskTrackerDbContext(options);
            _repository = new TaskRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddTaskToDatabase()
        {
            //arrange
            var task = new TaskItem
            {
                Title = "Test Repo Task",
                CreatedByUserId = 1,
                CreatedAt = DateTime.UtcNow
            };

            //act
            await _repository.AddAsync(task);
            await _context.SaveChangesAsync();

            //assert
            var savedTask = await _context.TaskItems.FirstOrDefaultAsync(t => t.Title == "Test Repo Task");
            savedTask.Should().NotBeNull();
            savedTask!.Id.Should().BeGreaterThan(0);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
