using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Interfaces;
using TaskTracker.Infrastructure.Data;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;


namespace TaskTracker.Infrastructure.Repositories
{
    public class TaskRepository : Repository<TaskItem>, ITaskRepository
    {
        public TaskRepository(TaskTrackerDbContext context) : base(context)
        {
        }

        public async Task<TaskItem?> GetWithCommentsAsync(int id)
        {
            return await _dbSet
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IReadOnlyList<TaskItem>> ListAsync(ISpecification<TaskItem> spec)
        {
            var specificationResult = SpecificationEvaluator.Default.GetQuery(_dbSet.AsQueryable(), spec);
            return await specificationResult.ToListAsync();
        }
        public async Task<int> CountAsync(ISpecification<TaskItem> spec)
        {
            
            var specificationResult = SpecificationEvaluator.Default.GetQuery(_dbSet.AsQueryable(), spec, true);
            return await specificationResult.CountAsync();
        }
    }
}
