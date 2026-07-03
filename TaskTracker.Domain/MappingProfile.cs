using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Domain.DTOs.Comments;
using TaskTracker.Domain.DTOs.Tasks;
using TaskTracker.Domain.DTOs.Users;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<TaskItem, TaskItemResponse>();
            CreateMap<TaskItem, TaskWithCommentsResponse>();
            CreateMap<CreateTaskRequest, TaskItem>();
            CreateMap<UpdateTaskRequest, TaskItem>();
            CreateMap<Comment, CommentResponse>();
            CreateMap<CreateCommentRequest, Comment>();
        }
    }
}