using FastEndpoints;
using Orders.Fastendpoints.Infrastructure;
using Orders.Fastendpoints.Interfaces;

namespace Orders.Fastendpoints.Features.OrderManagement;

public class GetOrderEndpoint(IOrderService orderService)
    : Endpoint<GetOrderEndpoint.GetOrdersRequest, List<Data.Models.Order>>
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
        PostProcessor<TimingPostProcessor<GetOrdersRequest, List<Data.Models.Order>>>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetOrdersRequest request, CancellationToken ct)
    {
        if (request.PageSize <= 0 || request.PageSize > 10000)
        {
            await Send.ErrorsAsync();
        }

        var orders = orderService.GetOrders(request.PageSize).Result;

        await Send.OkAsync(orders);
    }
}