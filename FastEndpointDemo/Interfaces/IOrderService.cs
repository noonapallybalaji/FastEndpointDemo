using DataAccess.Models;
using WebAPI.RequestModels.OrderRequestModels;

namespace WebAPI.Interfaces;

public interface IOrderService
{
    Task<List<Order>> GetOrders(int pageSize);

    Task<Order> CreateOrder(CreateOrderRequestModel request);

    Task<Order?> UpdateOrder(int id, UpdateOrderRequestModel request);

    Task<bool> DeleteOrder(int id);
}