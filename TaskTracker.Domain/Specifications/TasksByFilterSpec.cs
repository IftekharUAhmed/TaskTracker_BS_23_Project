using Ardalis.Specification;
using System;
using TaskTracker.Domain.DTOs.Tasks;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Specifications
{
    public class TasksByFilterSpec : Specification<TaskItem>
    {
        public TasksByFilterSpec(TaskFilterRequest filter)
        {

            if (filter.Status.HasValue)
            {
                Query.Where(t => t.Status == filter.Status.Value);
            }

            if (filter.Priority.HasValue)
            {
                Query.Where(t => t.Priority == filter.Priority.Value);
            }

            if (filter.AssigneeId.HasValue)
            {
                Query.Where(t => t.AssignedUserId == filter.AssigneeId.Value);
            }

            if (filter.DueBefore.HasValue)
            {
                Query.Where(t => t.DueDate <= filter.DueBefore.Value);
            }

            if (filter.DueAfter.HasValue)
            {
                Query.Where(t => t.DueDate >= filter.DueAfter.Value);
            }
            if (filter.IncludeDeleted)
            {
                Query.IgnoreQueryFilters();
            }


            Query.Skip((filter.Page - 1) * filter.PageSize)
                 .Take(filter.PageSize);
            Query.OrderByDescending(t => t.CreatedAt);
        }
    }
}