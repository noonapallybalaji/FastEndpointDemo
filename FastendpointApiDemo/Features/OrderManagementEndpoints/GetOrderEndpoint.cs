using DataAccess.DataBaseContext;
using FastendpointApiDemo.Infrastructure;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace FastendpointApiDemo.Features.OrderManagementEndpoints;

public class GetOrderEndpoint(AppDbContext context)
    : Endpoint<GetOrderEndpoint.GetOrdersRequest, List<DataAccess.Models.Order>>
{
    public class GetOrdersRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 100;
    }
    public override void Configure()
    {
        Get("/orders");
        PreProcessor<TimingPreProcessor<GetOrdersRequest>>();
        PostProcessor<TimingPostProcessor<GetOrdersRequest, List<DataAccess.Models.Order>>>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetOrdersRequest request, CancellationToken ct)
    {
        if (request.PageSize <= 0 || request.PageSize > 10000)
        {
            await Send.ErrorsAsync();
        }

        var orders = await context.Orders
            .AsNoTracking()
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        await Send.OkAsync(orders);
    }
}