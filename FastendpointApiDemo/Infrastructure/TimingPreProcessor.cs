namespace FastendpointApiDemo.Infrastructure;

using System.Diagnostics;
using FastendpointApiDemo.State;
using FastEndpoints;

public class TimingPreProcessor<TRequest>(
    ILogger<TimingPreProcessor<TRequest>> logger)
    : PreProcessor<TRequest, RequestTimingState>
{
    public override Task PreProcessAsync(
        IPreProcessorContext<TRequest> context,
        RequestTimingState state,
        CancellationToken ct)
    {
        // Start high-resolution timer
        state.Stopwatch = Stopwatch.StartNew();

        // Minimal logging (avoid heavy string creation)
        logger.LogInformation(
            "Request started for {Path}",
            context.HttpContext.Request.Path);

        return Task.CompletedTask;
    }
}