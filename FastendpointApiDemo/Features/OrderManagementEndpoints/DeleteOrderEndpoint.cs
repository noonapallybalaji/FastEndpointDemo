using DataAccess.DataBaseContext;
using FastEndpoints;
using static FastendpointApiDemo.Features.OrderManagementEndpoints.DeleteOrderEndpoint;

namespace FastendpointApiDemo.Features.OrderManagementEndpoints
{
    public class DeleteOrderEndpoint(AppDbContext context) : Endpoint<DeleteOrderRequest>
    {
        public class DeleteOrderRequest
        {
            public int Id { get; set; }
        }

        public override void Configure()
        {
            Delete("/orders/{Id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(DeleteOrderRequest req, CancellationToken ct)
        {
            var order = await context.Orders.FindAsync(req.Id);

            if (order == null)
            {
                await Send.NotFoundAsync(); 
                return;
            }

            context.Orders.Remove(order);
            await context.SaveChangesAsync(ct);

            await Send.OkAsync(
                response: "Order Successfully deleted",
                cancellation: ct);
        }

    }
}
