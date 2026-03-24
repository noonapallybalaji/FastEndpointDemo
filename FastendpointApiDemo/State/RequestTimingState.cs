using System.Diagnostics;

namespace Orders.Fastendpoints.State;

public class RequestTimingState
{
    public Stopwatch Stopwatch { get; set; } = new();
}
