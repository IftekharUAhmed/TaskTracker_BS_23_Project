using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using System.Threading.Tasks;
using TaskTracker.Domain.DTOs.Comments;
using TaskTracker.Domain.DTOs.Common;
using TaskTracker.Domain.DTOs.Tasks;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Exceptions;
using TaskTracker.Domain.Interfaces;
using TaskTracker.Domain.Specifications;

namespace TaskTracker.Domain.Managers
{
    public class TaskManager : ITaskManager
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TaskManager(ITaskRepository taskRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<PagedResponse<TaskItemResponse>> GetTasksAsync(TaskFilterRequest filter, string currentUserRole)
        {
            if (filter.Page < 1) filter.Page = 1;
            if (filter.PageSize < 1) filter.PageSize = 20;
            if (filter.PageSize > 100) filter.PageSize = 100;
            if (filter.IncludeDeleted && currentUserRole != UserRoles.Admin)
            {
                filter.IncludeDeleted = false;
            }

            var spec = new TasksByFilterSpec(filter);

            //count specification without pagination
            var countFilter = new TaskFilterRequest
            {
                Status = filter.Status,
                Priority = filter.Priority,
                AssigneeId = filter.AssigneeId,
                DueBefore = filter.DueBefore,
                DueAfter = filter.DueAfter,
                IncludeDeleted = filter.IncludeDeleted
            };
            var countSpec = new TasksByFilterSpec(countFilter);

            var tasks = await _taskRepository.ListAsync(spec);
            var totalItems = await _taskRepository.CountAsync(countSpec);

            var mappedTasks = _mapper.Map<IEnumerable<TaskItemResponse>>(tasks);

            return new PagedResponse<TaskItemResponse>
            {
                Items = mappedTasks,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)filter.PageSize)
            };
        }

        public async Task<TaskWithCommentsResponse> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetWithCommentsAsync(id);
            if (task == null) throw new NotFoundException($"Task with ID {id} not found.");

            return _mapper.Map<TaskWithCommentsResponse>(task);
        }

        public async Task<TaskItemResponse> CreateTaskAsync(CreateTaskRequest request, int creatorUserId)
        {
            var task = _mapper.Map<TaskItem>(request);

            task.CreatedByUserId = creatorUserId;
            task.CreatedAt = DateTime.UtcNow;
            task.Status = Enums.TaskStatus.Todo;

            await _taskRepository.AddAsync(task);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TaskItemResponse>(task);
        }

        public async Task UpdateTaskAsync(int id, UpdateTaskRequest request, int currentUserId, string currentUserRole)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) throw new NotFoundException($"Task with ID {id} not found.");

            if (task.CreatedByUserId != currentUserId && currentUserRole != UserRoles.Admin && currentUserRole != UserRoles.Manager)
            {
                throw new ForbiddenException("You do not have permission to update this task.");
            }

            _mapper.Map(request, task);
            task.UpdatedAt = DateTime.UtcNow;

            _taskRepository.Update(task);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int id, int currentUserId, string currentUserRole)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) throw new NotFoundException($"Task with ID {id} not found.");

            if (task.CreatedByUserId != currentUserId && currentUserRole != UserRoles.Admin && currentUserRole != UserRoles.Manager)
            {
                throw new ForbiddenException("You do not have permission to delete this task.");
            }

            task.IsDeleted = true;
            task.UpdatedAt = DateTime.UtcNow;

            _taskRepository.Update(task);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddCommentAsync(int taskId, CreateCommentRequest request, int authorUserId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) throw new NotFoundException($"Task with ID {taskId} not found.");

            var comment = _mapper.Map<Comment>(request);
            comment.TaskItemId = taskId;
            comment.AuthorUserId = authorUserId;
            comment.CreatedAt = DateTime.UtcNow;

            task.Comments.Add(comment);

            _taskRepository.Update(task);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}