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
        Task<List<OrderDetailDto>> GetOrderDetailsByOrderId(int orderId);
        Task<Order> GetOrderByOrderId(int orderId);
        Task DeleteOrderAsync(int orderId);
        Task<List<OrderDto>> GetOrdersByPeriod(DateTime startDate, DateTime endDate);
        Task<List<OrderDto>> GetOrdersByPeriod(DateTime startDate, DateTime endDate, string staffName);

        Task<List<OrderDto>> GetOrdersByOrderDate(DateTime orderDate, string staffName);
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByNameAsync(string productName);
    }
}