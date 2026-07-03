using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using TaskTracker.Domain.DTOs.Common;
using TaskTracker.Domain.DTOs.Users;
using TaskTracker.Domain.Exceptions;
using TaskTracker.Domain.Interfaces;

namespace TaskTracker.Domain.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserManager(IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserResponse> GetCurrentUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<PagedResponse<UserResponse>> GetPagedUsersAsync(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 100) pageSize = 100;

            var allUsers = await _userRepository.GetAllAsync();
            var query = allUsers.AsQueryable();

            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pagedEntities = query
                .OrderBy(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var mappedItems = _mapper.Map<IEnumerable<UserResponse>>(pagedEntities);

            return new PagedResponse<UserResponse>
            {
                Items = mappedItems,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        public async Task ChangeUserRoleAsync(int targetUserId, UpdateUserRoleRequest request)
        {
            var user = await _userRepository.GetByIdAsync(targetUserId);

            if (user == null)
            {
                throw new NotFoundException($"User with ID {targetUserId} not found.");
            }

            user.Role = request.Role;

            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

