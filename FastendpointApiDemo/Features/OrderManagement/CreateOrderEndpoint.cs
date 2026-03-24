using FastEndpoints;
using FluentValidation;
using Orders.ExternalServices.Interfaces;
using Orders.Fastendpoints.Interfaces;

namespace Orders.Fastendpoints.Features.OrderManagement;

public class CreateOrderRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class CreateOrderResponse
{
    public string? Message { get; set; }
}

public class Validator : Validator<CreateOrderRequest>
{
    public Validator()
    {
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}

public class CreateOrderEndpoint(IOrderService orderService, IEmailService emailService, ICacheService cacheService) 
    : Endpoint<CreateOrderRequest, CreateOrderResponse>
{
    public override void Configure()
    {
        Post("/orders");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateOrderRequest request, CancellationToken ct)
    {
        var result = orderService.CreateOrder(request);
        cacheService.CreateCache(result);
        emailService.Send();

        Response = new CreateOrderResponse
        {
            Message = "Order created successfully"
        };

    }
}
