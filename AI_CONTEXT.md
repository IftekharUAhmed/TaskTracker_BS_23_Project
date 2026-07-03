# TaskTracker Project Context

## Project Overview

Project Name:
TaskTracker

Project Type:
Production-grade RESTful API

Purpose:
A task management system where users can register, authenticate, create and manage tasks, add comments, and administrators can manage users and roles.


# Technology Stack

Backend:
- ASP.NET Core Web API

Database:
- PostgreSQL 16

ORM:
- Entity Framework Core

Authentication:
- JWT Access Token
- Refresh Token Rotation

Testing:
- xUnit

Deployment:
- Docker


# Solution Structure

TaskTracker.API

Responsibility:
- Controllers
- Middleware
- Program.cs
- Authentication configuration
- Swagger configuration
- API endpoints


TaskTracker.Domain

Responsibility:
- Entities
- DTOs
- Enums
- Validators
- Business logic
- Manager classes
- Interfaces
- Custom Exceptions


TaskTracker.Infrastructure

Responsibility:
- DbContext
- EF Core configuration
- Migrations
- Repository implementations
- UnitOfWork implementation
- JWT Token Generator
- Password hashing


TaskTracker.Tests

Responsibility:
- Unit Tests
- Integration Tests



# Architecture Rules

1. Domain layer must not depend on:
- API
- Infrastructure
- EF Core
- ASP.NET


2. API Controllers must not:
- Access DbContext
- Access Repository directly
- Contain business logic


3. Controller flow:

Controller
    ↓
Manager Interface
    ↓
Repository Interface
    ↓
Infrastructure Implementation
    ↓
Database


4. Entities must never be returned directly from API.

Always use DTO.


5. Business logic classes must be named Manager.

Example:

Correct:
- TaskManager
- AuthManager
- UserManager

Wrong:
- TaskService
- AuthService



# Domain Entities


## User

Properties:

- Id
- Username
- Email
- PasswordHash
- Role
- CreatedAt
- RefreshTokens


Role values:

- Admin
- Manager
- Member



## RefreshToken

Properties:

- Id
- Token
- UserId
- ExpiresAt
- RevokedAt
- ReplacedByToken



## TaskItem

Note:
Use TaskItem name to avoid conflict with System.Threading.Tasks.Task


Properties:

- Id
- Title
- Description
- Status
- Priority
- DueDate
- AssignedUserId
- CreatedByUserId
- CreatedAt
- UpdatedAt
- IsDeleted
- Comments



## Comment

Properties:

- Id
- TaskItemId
- AuthorUserId
- Body
- CreatedAt



# Main Features


Authentication:

- Register
- Login
- Refresh Token
- Logout


Users:

- Get current user
- Admin can view users
- Admin can change role


Tasks:

- Create task
- View task list
- Filter tasks
- Pagination
- Update task
- Soft delete task
- View task with comments


Comments:

- Add comment to task



# Development Status


Completed:

- Solution setup


Current:

- Waiting for Domain implementation


Pending:

- Entities
- DTOs
- Managers
- Repository
- Database
- Authentication
- Controllers
- Tests
- Docker



# AI Behavior Rules

When helping:

- Analyze before coding
- Explain before creating files
- Maintain existing architecture
- Do not introduce unnecessary patterns
- Ask before changing decisions