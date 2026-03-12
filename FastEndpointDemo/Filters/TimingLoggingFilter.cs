namespace WebAPI.Filters;

using System.Diagnostics;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPI.RequestModels.OrderRequestModels;

public class TimingLoggingFilter(ILogger<TimingLoggingFilter> logger) : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var state = new RequestTimingState
        {
            Stopwatch = Stopwatch.StartNew(),
            StartTime = DateTime.UtcNow
        };

        context.HttpContext.Items["TimingState"] = state;

        // Extract headers (common in logging middleware)
        var headers = context.HttpContext.Request.Headers;

        string correlationId = headers["X-Correlation-ID"].FirstOrDefault()
                               ?? Guid.NewGuid().ToString();

        // Extract claims (very common in enterprise APIs)
        var userId = context.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Build structured request log
        StringBuilder requestPayload = new();

        foreach (var arg in context.ActionArguments)
        {
            object value = arg.Value!;

            var type = value.GetType();

            requestPayload.AppendLine($"Argument: {arg.Key}");

            foreach (PropertyInfo prop in type.GetProperties())
            {
                var propertyValue = prop.GetValue(value);

                requestPayload.AppendLine($"{prop.Name}: {propertyValue}");
            }
        }

        logger.LogInformation(
            """
            Request Started
            Path: {Path}
            CorrelationId: {CorrelationId}
            UserId: {UserId}
            Payload: {Payload}
            """,
            context.HttpContext.Request.Path,
            correlationId,
            userId,
            requestPayload.ToString());
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.HttpContext.Items.TryGetValue("TimingState", out var stateObj)
            && stateObj is RequestTimingState state)
        {
            state.Stopwatch.Stop();

            logger.LogInformation(
                "Request {Path} completed in {Duration} ms",
                context.HttpContext.Request.Path,
                state.Stopwatch.ElapsedMilliseconds);
        }
    }
}