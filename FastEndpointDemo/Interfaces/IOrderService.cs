using DataAccess.Models;

namespace WebAPI.Interfaces
{
    public interface IOrderService
    {
        /// <summary>
        /// 
        /// </summary>
        Task<List<Order>> GetOrder(int PageSize);
    }
}
