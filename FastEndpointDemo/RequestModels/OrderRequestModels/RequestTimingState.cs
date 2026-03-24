using System.Diagnostics;

namespace Orders.Web.RequestModels.OrderRequestModels;

public class RequestTimingState
{
    public Stopwatch Stopwatch { get; set; } = new();
    public DateTime StartTime { get; set; }
}
