using MedicalClinic.ManagementSystem.Domain.Models.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;

namespace MedicalClinic.ManagementSystem.Api.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(builder =>
        {
            builder.Run(async context =>
            {
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    var problemDetailsFactory = app.Services.GetRequiredService<ProblemDetailsFactory>();
                    var logger = app.Services.GetRequiredService<ILoggerFactory>()
                        .CreateLogger("GlobalExceptionHandler");
                    var environment = app.Services.GetRequiredService<IWebHostEnvironment>();
                    var exception = contextFeature.Error;

                    var (statusCode, title) = exception switch
                    {
                        ValidationException => (StatusCodes.Status400BadRequest, "Validation Failed"),
                        BadRequestException badRequest => (StatusCodes.Status400BadRequest, badRequest.Title),
                        NotFoundException notFound => (StatusCodes.Status404NotFound, notFound.Title),
                        TokenValidationException tokenEx => (tokenEx.StatusCode, "Unauthorized"),
                        SecurityTokenException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
                        UnauthorizedAccessException => (StatusCodes.Status403Forbidden, "Forbidden"),
                        _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
                    };

                    if (statusCode >= StatusCodes.Status500InternalServerError)
                    {
                        logger.LogError(exception, "Unhandled exception occurred while processing {Path}.", context.Request.Path);
                    }
                    else
                    {
                        logger.LogWarning(exception, "Handled exception occurred while processing {Path}.", context.Request.Path);
                    }

                    var detail = statusCode >= StatusCodes.Status500InternalServerError && !environment.IsDevelopment()
                        ? "An unexpected error occurred."
                        : exception.Message;

                    var problemDetails = problemDetailsFactory.CreateProblemDetails(
                        context,
                        statusCode,
                        title: title,
                        detail: detail,
                        instance: context.Request.Path);

                    problemDetails.Extensions["traceId"] = context.TraceIdentifier;

                    context.Response.StatusCode = statusCode;
                    context.Response.ContentType = "application/problem+json";
                    await context.Response.WriteAsJsonAsync(problemDetails);
                }
            });
        });
    }
}
