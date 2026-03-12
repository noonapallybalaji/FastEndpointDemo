using DataAccess.DataBaseContext;
using DataAccess.Models;
using FastendpointApiDemo.Infrastructure;
using FastEndpoints;

namespace FastendpointApiDemo.Features.OrderManagementEndpoints
{
    public class CreateOrderEndpoint(AppDbContext context) 
        : Endpoint<CreateOrderEndpoint.CreateOrderRequest, CreateOrderEndpoint.CreateOrderResponse>
    {
        public class CreateOrderRequest
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

        public class CreateOrderResponse
        {
            public string? Message { get; set; }
        }

        public override void Configure()
        {
            Post("/orders");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateOrderRequest req, CancellationToken ct)
        {
            DataAccess.Models.Order order = new DataAccess.Models.Order
            {
                ProductId = req.ProductId,
                Quantity = req.Quantity,
                CreatedDate = DateTime.UtcNow
            };

            context.Orders.Add(order);
            await context.SaveChangesAsync(ct);

            Response = new CreateOrderResponse
            {
                Message = "Order created successfully"
            };

        }
    }
}
