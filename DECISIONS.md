# TaskTracker Architecture Decisions


## Decision 1

Architecture:

3 Tier Architecture


Reason:

Project requirement explicitly requires separation between API, Domain and Infrastructure.



## Decision 2

Business Logic Pattern:

Manager Pattern


Reason:

Requirement specifies Manager classes instead of Service classes.



Examples:

- AuthManager
- TaskManager
- UserManager



## Decision 3

Database:

PostgreSQL 16


Reason:

Required by project specification.



## Decision 4

ORM:

Entity Framework Core


Reason:

Required for database access and migrations.



## Decision 5

Database Access Pattern:

Repository + UnitOfWork


Reason:

To separate database operations from business logic.



## Decision 6

Filtering:

Specification Pattern


Reason:

Task filtering should not create large if-condition chains inside repositories.



## Decision 7

Delete Strategy:

Soft Delete


Implementation:

IsDeleted flag

EF Core Global Query Filter


Reason:

Deleted data should remain recoverable.



## Decision 8

Authentication:

JWT Access Token + Refresh Token


Access Token:
15 minutes


Refresh Token:
7 days


Refresh tokens stored in database.



## Decision 9

Error Handling:

RFC 7807 ProblemDetails


Reason:

Standard API error response format.



## Decision 10

API Contract:

DTO based


Reason:

Entities should never directly expose database structure.

## Decision:

Entity to DTO mapping will happen inside Manager layer.

Reason:

Follow instructor Module 17 approach.

Decision:

DTO validation will be handled using FluentValidation.

Reason:

Project requirement priority > instructor module data annotation approach.