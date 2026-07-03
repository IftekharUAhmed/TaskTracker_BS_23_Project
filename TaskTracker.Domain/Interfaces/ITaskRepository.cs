using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Domain.Entities;
using Ardalis.Specification;

namespace TaskTracker.Domain.Interfaces
{
    public interface ITaskRepository : IRepository<TaskItem>
    {
        Task<TaskItem?> GetWithCommentsAsync(int id);
        Task<IReadOnlyList<TaskItem>> ListAsync(ISpecification<TaskItem> spec);
        Task<int> CountAsync(ISpecification<TaskItem> spec);
    }
}