using FastEndpoints;
using Orders.Data.DataBaseContext;

namespace Orders.Fastendpoints.Features.OrderManagement;

public class UpdateOrderRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class PatchOrderEndpoint(AppDbContext context)
    : Endpoint<UpdateOrderRequest>
{
    public override void Configure()
    {
        Patch("/orders");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateOrderRequest req, CancellationToken ct)
    {
        var order = await context.Orders.FindAsync(req.ProductId);

        if (order == null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        order.Quantity = req.Quantity;

        await context.SaveChangesAsync(ct);

        await Send.OkAsync("Order updated successfully");
    }
}
