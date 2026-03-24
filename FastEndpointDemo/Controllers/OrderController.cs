using Microsoft.AspNetCore.Mvc;
using Orders.ExternalServices.Interfaces;
using Orders.Web.Filters;
using Orders.Web.Interfaces;
using Orders.Web.RequestModels.OrderRequestModels;

namespace Orders.Web.Controllers;

/// <summary>
/// Controller responsible for managing Order resources.
/// Provides endpoints for retrieving, creating, updating, and deleting orders.
/// </summary>
/// <param name="orderService">Service responsible for order business logic.</param>
[ApiController]
[Route("api/orders")]
[ServiceFilter(typeof(TimingLoggingFilter))]
public class OrdersController(IOrderService orderService, IEmailService emailService, ICacheService cacheService, IAuditService auditService) : ControllerBase
{
    /// <summary>
    /// Retrieves a paginated list of orders.
    /// </summary>
    /// <param name="request">Pagination request containing page and page size.</param>
    /// <returns>A list of orders.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Data.Models.Order>>> GetOrders([FromQuery] GetOrderRequestModel request)
    {
        if (request.PageSize <= 0 || request.PageSize > 10000)
        {
            return BadRequest("PageSize must be between 1 and 10000");
        }

        var orders = await orderService.GetOrders(request.PageSize);

        return Ok(orders);
    }

    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <param name="request">Order creation request.</param>
    /// <returns>The created order.</returns>
    [HttpPost]
    public async Task<ActionResult<Data.Models.Order>> CreateOrder([FromBody] CreateOrderRequestModel request)
    {
        if (request.Quantity <= 0)
        {
            return BadRequest("Quantity must be greater than zero.");
        }

        var createdOrder = await orderService.CreateOrder(request);

        cacheService.CreateCache(createdOrder);
        emailService.Send();

        return CreatedAtAction(nameof(GetOrders), new { id = createdOrder.Id }, createdOrder);
    }

    /// <summary>
    /// Updates an existing order.
    /// </summary>
    /// <param name="id">Order identifier.</param>
    /// <param name="request">Update request.</param>
    /// <returns>Updated order.</returns>
    [HttpPatch("{id}")]
    public async Task<ActionResult<Data.Models.Order>> UpdateOrder(int id, [FromBody] UpdateOrderRequestModel request)
    {
        Data.Models.Order? updatedOrder = await orderService.UpdateOrder(id, request);

        if (updatedOrder == null)
        {
            return NotFound($"Order with Id {id} not found.");
        }

        auditService.Log("Order Updated!");

        return Ok(updatedOrder);
    }

    /// <summary>
    /// Deletes an existing order.
    /// </summary>
    /// <param name="id">Order identifier.</param>
    /// <returns>No content if successful.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var deleted = await orderService.DeleteOrder(id);

        if (!deleted)
        {
            return NotFound($"Order with Id {id} not found.");
        }

        return NoContent();
    }
}