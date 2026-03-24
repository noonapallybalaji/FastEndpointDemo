using Orders.Data.Models;
using Orders.Fastendpoints.Features.OrderManagement;

namespace Orders.Fastendpoints.Interfaces;

public interface IOrderService
{
    Task<List<Order>> GetOrders(int pageSize);

    Task<Order> CreateOrder(CreateOrderRequest request);

    Task<Order?> UpdateOrder(int id, UpdateOrderRequest request);

    Task<bool> DeleteOrder(int id);
}