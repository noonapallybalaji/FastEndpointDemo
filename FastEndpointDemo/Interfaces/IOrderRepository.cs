using DataAccess.Models;
using WebAPI.RequestModels.OrderRequestModels;

namespace WebAPI.Interfaces;

public interface IOrderRepository
{
    Task<List<Order>> GetOrders(int pageSize);

    Task<Order> CreateOrder(Order order);

    Task<Order?> GetById(int id);

    Task DeleteOrder(Order order);

    Task SaveAsync();
}