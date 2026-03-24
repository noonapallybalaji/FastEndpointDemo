using Orders.Fastendpoints.State;
using FastEndpoints;

namespace Orders.Fastendpoints.Infrastructure;

public class TimingPostProcessor<TRequest, TResponse>(
    ILogger<TimingPostProcessor<TRequest, TResponse>> logger)
    : PostProcessor<TRequest, RequestTimingState, TResponse>
{
    public override Task PostProcessAsync(
        IPostProcessorContext<TRequest, TResponse> context,
        RequestTimingState state,
        CancellationToken ct)
    {
        state.Stopwatch.Stop();

        logger.LogInformation(
            "Request {Path} completed in {Duration} ms",
            context.HttpContext.Request.Path,
            state.Stopwatch.ElapsedMilliseconds);

        return Task.CompletedTask;
    }
}