using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;
using WebAPI.Interfaces;
using WebAPI.RequestModels.OrderRequestModels;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/orders")]
[ServiceFilter(typeof(TimingLoggingFilter))]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(
        IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult> GetOrders([FromQuery] GetOrderRequestModel request)
    {
        if (request.PageSize <= 0 || request.PageSize > 10000)
        {
            return BadRequest("PageSize must be between 1 and 10000");
        }

        var orders = await _orderService.GetOrder(request.PageSize);

        return Ok(orders);
    }
}