using DataAccess.DataBaseContext;
using DataAccess.Models;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace FastendpointApiDemo.Features.OrderManagementEndpoints
{
    public class PatchOrderEndpoint(AppDbContext context)
        : Endpoint<PatchOrderEndpoint.OrderRequest>
    {
        public class OrderRequest
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

        public override void Configure()
        {
            Patch("/orders");
            AllowAnonymous();
        }

        public override async Task HandleAsync(OrderRequest req, CancellationToken ct)
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
}
