using System.Net;
using System.Text.Json;
using ConsultantsSalary.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace ConsultantsSalary.Api.Shared;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteProblemAsync(context, ex);
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, Exception ex)
    {
        var problem = new ProblemDetails
        {
            Title = "An error occurred while processing your request.",
            Detail = ex.Message,
            Status = (int)HttpStatusCode.InternalServerError
        };

        switch (ex)
        {
            case DailyLimitExceededException dle:
                problem.Status = StatusCodes.Status400BadRequest;
                problem.Title = "Daily hour limit exceeded";
                problem.Extensions["consultantId"] = dle.ConsultantId;
                problem.Extensions["dateWorked"] = dle.DateWorked;
                problem.Extensions["attemptedTotalHours"] = dle.AttemptedTotalHours;
                break;
            case ArgumentOutOfRangeException:
            case InvalidOperationException:
                problem.Status = StatusCodes.Status400BadRequest;
                break;
            case KeyNotFoundException:
                problem.Status = StatusCodes.Status404NotFound;
                break;
        }

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
        var json = JsonSerializer.Serialize(problem);
        await context.Response.WriteAsync(json);
    }
}
