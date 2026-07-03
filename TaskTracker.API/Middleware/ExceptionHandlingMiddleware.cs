using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TaskTracker.Domain.Exceptions;

namespace TaskTracker.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";

            var statusCode = StatusCodes.Status500InternalServerError;
            var title = "Server Error";
            var detail = exception.Message;
            IDictionary<string, string[]> errors = null!;

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = StatusCodes.Status400BadRequest;
                    title = "Validation Error";
                    errors = validationException.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(x => x.ErrorMessage).ToArray()
                        );
                    detail = "One or more validation errors occurred.";
                    break;

                case NotFoundException _:
                    statusCode = StatusCodes.Status404NotFound;
                    title = "Resource Not Found";
                    break;

                case UnauthorizedAccessException _:
                    statusCode = StatusCodes.Status401Unauthorized;
                    title = "Unauthorized";
                    break;

                case ForbiddenException _:
                    statusCode = StatusCodes.Status403Forbidden;
                    title = "Forbidden";
                    break;

                case DbUpdateConcurrencyException _:
                    statusCode = StatusCodes.Status409Conflict;
                    title = "Concurrency Conflict";
                    detail = "The resource was updated by another process. Please reload and try again.";
                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    title = "Internal Server Error";
                    detail = _env.IsDevelopment() ? exception.StackTrace ?? exception.Message : "An unexpected error occurred.";
                    break;
            }

            context.Response.StatusCode = statusCode;

            //ffollow rfc 7807
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = context.Request.Path
            };

            if (errors != null)
            {
                problemDetails.Extensions["errors"] = errors;
            }

            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var result = JsonSerializer.Serialize(problemDetails, jsonOptions);

            await context.Response.WriteAsync(result);
        }
    }
}