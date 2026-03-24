using Orders.Data.DataBaseContext;
using Orders.Data.Models;
using Microsoft.EntityFrameworkCore;
using Orders.Web.Interfaces;

namespace Orders.Web.Repositories;

/// <summary>
/// Repository responsible for handling database operations related to Orders.
/// </summary>
/// <param name="context">Application database context.</param>
public class OrderRepository(AppDbContext context) : IOrderRepository
{
    /// <summary>
    /// Retrieves a limited number of orders from the database.
    /// </summary>
    /// <param name="pageSize">Number of orders to retrieve.</param>
    /// <returns>List of orders.</returns>
    public async Task<List<Order>> GetOrders(int pageSize)
    {
        return await context.Orders
            .AsNoTracking()
            .Take(pageSize)
            .ToListAsync();
    }

    /// <summary>
    /// Creates a new order and adds it to the database.
    /// </summary>
    /// <param name="order">Order entity to create.</param>
    /// <returns>The created order.</returns>
    public async Task<Order> CreateOrder(Order order)
    {
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        return order;
    }

    /// <summary>
    /// Retrieves an order by its identifier.
    /// </summary>
    /// <param name="id">Order identifier.</param>
    /// <returns>Order if found; otherwise null.</returns>
    public async Task<Order?> GetById(int id)
    {
        return await context.Orders.FindAsync(id);
    }

    /// <summary>
    /// Deletes an order from the database.
    /// </summary>
    /// <param name="order">Order entity to delete.</param>
    public async Task DeleteOrder(Order order)
    {
        context.Orders.Remove(order);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Persists changes to the database.
    /// </summary>
    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
}