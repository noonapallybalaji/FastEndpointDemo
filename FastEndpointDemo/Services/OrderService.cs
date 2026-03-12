using Azure.Core;
using DataAccess.DataBaseContext;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using WebAPI.Interfaces;

namespace WebAPI.Services
{
    public class OrderService(AppDbContext context) : IOrderService
    {
        public async Task<List<Order>> GetOrder(int PageSize)
        {
            return await context.Orders
            .AsNoTracking()
            .Take(PageSize)   // limit results
            .ToListAsync();
        }

    }
}
