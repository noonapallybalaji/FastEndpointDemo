using FastEndpoints;
using Orders.Fastendpoints.Interfaces;

namespace Orders.Fastendpoints.Features.OrderManagementEndpoints;
public class DeleteOrderRequest
{
    public int Id { get; set; }
}
public class DeleteOrderEndpoint(IOrderService orderService) : Endpoint<DeleteOrderRequest>
{
    public override void Configure()
    {
        Delete("/orders/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteOrderRequest req, CancellationToken ct)
    {
        _ = await orderService.DeleteOrder(req.Id);

        await Send.OkAsync(
            response: "Order Successfully deleted",
            cancellation: ct);
    }

}
