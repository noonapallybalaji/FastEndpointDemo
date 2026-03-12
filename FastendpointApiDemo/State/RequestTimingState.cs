using System.Diagnostics;

namespace FastendpointApiDemo.State;

public class RequestTimingState
{
    public Stopwatch Stopwatch { get; set; } = new();
}
