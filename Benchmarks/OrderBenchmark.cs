using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
namespace Benchmarks;
public class ApiBenchmark
{
    private static readonly HttpClient httpClient = new HttpClient();

    private const string FastEndpointUrl = "http://localhost:5244/orders?page=1&pageSize=100";
    private const string WebApiUrl = "http://localhost:5267/api/orders?pageSize=100";

    private const int Iterations = 50;

    public static async Task RunAsync()
    {
        Console.WriteLine("Starting benchmark...");
        Console.WriteLine();

        // Warmup
        Console.WriteLine("Warming up endpoints...");
        await httpClient.GetAsync(FastEndpointUrl);
        await httpClient.GetAsync(WebApiUrl);
        Console.WriteLine("Warmup done.");
        Console.WriteLine();

        // Force GC cleanup
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        double fastEndpointTotal = 0;
        double webApiTotal = 0;

        double fastMin = double.MaxValue;
        double webMin = double.MaxValue;

        double fastMax = 0;
        double webMax = 0;

        Stopwatch stopwatch = new Stopwatch();

        Console.WriteLine($"Running {Iterations} iterations...");
        Console.WriteLine();

        for (int i = 0; i < Iterations; i++)
        {
            // FAST ENDPOINT
            stopwatch.Restart();
            var fastResponse = await httpClient.GetAsync(FastEndpointUrl);
            stopwatch.Stop();

            fastResponse.EnsureSuccessStatusCode();

            double fastTime = stopwatch.Elapsed.TotalMilliseconds;

            fastEndpointTotal += fastTime;
            fastMin = Math.Min(fastMin, fastTime);
            fastMax = Math.Max(fastMax, fastTime);

            // WEB API
            stopwatch.Restart();
            var webResponse = await httpClient.GetAsync(WebApiUrl);
            stopwatch.Stop();

            webResponse.EnsureSuccessStatusCode();

            double webTime = stopwatch.Elapsed.TotalMilliseconds;

            webApiTotal += webTime;
            webMin = Math.Min(webMin, webTime);
            webMax = Math.Max(webMax, webTime);
        }

        double fastAvg = fastEndpointTotal / Iterations;
        double webAvg = webApiTotal / Iterations;

        Console.WriteLine();
        Console.WriteLine("===== RESULTS =====");
        Console.WriteLine();

        Console.WriteLine("FastEndpoints");
        Console.WriteLine($"Average: {fastAvg:F2} ms");
        Console.WriteLine($"Min: {fastMin:F2} ms");
        Console.WriteLine($"Max: {fastMax:F2} ms");
        Console.WriteLine();

        Console.WriteLine("Web API");
        Console.WriteLine($"Average: {webAvg:F2} ms");
        Console.WriteLine($"Min: {webMin:F2} ms");
        Console.WriteLine($"Max: {webMax:F2} ms");
        Console.WriteLine();

        Console.WriteLine("Difference");
        Console.WriteLine($"{Math.Abs(webAvg - fastAvg):F2} ms");

        if (fastAvg < webAvg)
        {
            Console.WriteLine("FastEndpoints is faster.");
        }
        else
        {
            Console.WriteLine("Web API is faster.");
        }
    }
}
