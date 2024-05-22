using Estore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Estore.Repositories.OrderRepository;

namespace Estore.Repositories
{
    public interface IOrderRepository
    {
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task AddOrderAsync(Order order);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderId(int orderId);
        Task<Order> GetOrderByOrderId(int orderId);
        Task DeleteOrderAsync(int orderId);
        Task<List<OrderDto>> GetOrdersByPeriod(DateTime startDate, DateTime endDate);
        Task<List<OrderDto>> GetOrdersByPeriod(DateTime startDate, DateTime endDate, string staffName);
    }
}