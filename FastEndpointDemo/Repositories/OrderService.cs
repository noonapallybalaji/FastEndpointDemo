
using Orders.Data.Models;
using Orders.Web.Interfaces;
using Orders.Web.RequestModels.OrderRequestModels;

namespace Orders.Web.Services;

/// <summary>
/// Service layer responsible for business logic related to Orders.
/// </summary>
/// <param name="orderRepository">Repository used to access order data.</param>
public class OrderService(IOrderRepository orderRepository) : IOrderService
{
    /// <summary>
    /// Retrieves a limited number of orders.
    /// </summary>
    /// <param name="pageSize">Number of records to fetch.</param>
    /// <returns>List of orders.</returns>
    public async Task<List<Order>> GetOrders(int pageSize)
    {
        return await orderRepository.GetOrders(pageSize);
    }

    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <param name="request">Request model containing order details.</param>
    /// <returns>The created order.</returns>
    public async Task<Order> CreateOrder(CreateOrderRequestModel request)
    {
        var order = new Order
        {
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            CreatedDate = DateTime.UtcNow
        };

        return await orderRepository.CreateOrder(order);
    }

    /// <summary>
    /// Updates an existing order.
    /// </summary>
    /// <param name="id">Order identifier.</param>
    /// <param name="request">Update request.</param>
    /// <returns>Updated order or null if not found.</returns>
    public async Task<Order?> UpdateOrder(int id, UpdateOrderRequestModel request)
    {
        var order = await orderRepository.GetById(id);

        if (order == null)
            return null;

        order.Quantity = request.Quantity;

        await orderRepository.SaveAsync();

        return order;
    }

    /// <summary>
    /// Deletes an order.
    /// </summary>
    /// <param name="id">Order identifier.</param>
    /// <returns>True if deleted, otherwise false.</returns>
    public async Task<bool> DeleteOrder(int id)
    {
        var order = await orderRepository.GetById(id);

        if (order == null)
            return false;

        await orderRepository.DeleteOrder(order);
        await orderRepository.SaveAsync();

        return true;
    }
}
