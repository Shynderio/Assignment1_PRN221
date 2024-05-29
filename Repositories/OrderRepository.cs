using Estore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Estore.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        // Simulating a data source, replace this with actual data retrieval logic
        private readonly MyStoreContext _storeContext;
        public OrderRepository(MyStoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            return await _storeContext.Orders
                .Include(o => o.Staff)
                .Include(o => o.OrderDetails)
                .Select(order => new OrderDto
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    EndDate = order.OrderDate.AddMonths(1),
                    StaffName = order.Staff.Name,
                    TotalPrice = order.OrderDetails.Sum(od => od.Quantity * od.UnitPrice)
                })
                .ToListAsync();

        }
        public async Task AddOrderAsync(Order order)
        {
            _storeContext.Orders.Add(order);
            await _storeContext.SaveChangesAsync();
        }

        public async Task<Order> GetOrderByOrderId(int orderId)
        {
            var order = _storeContext.Orders.Include(o => o.Staff).FirstOrDefault(o => o.OrderId == orderId);

            return order;
        }
        public async Task<List<OrderDetailDto>> GetOrderDetailsByOrderId(int orderId)
        {
            var orderDetails = await _storeContext.OrderDetails
                .Where(od => od.OrderId == orderId)
                .Include(od => od.Product)
                .ToListAsync();

            return orderDetails.Select(od => new OrderDetailDto
            {
                OrderDetailId = od.OrderDetailId,
                OrderId = od.OrderId,
                ProductId = od.ProductId,
                ProductName = od.Product.ProductName,
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice,
                TotalPrice = od.Quantity * od.UnitPrice
            }).ToList();
        }


        public async Task DeleteOrderAsync(int orderId)
        {
            try
            {
                var orderToDelete = await _storeContext.Orders.FindAsync(orderId);
                if (orderToDelete != null)
                {
                    _storeContext.Orders.Remove(orderToDelete);
                    await _storeContext.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentException("Order not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task<List<OrderDto>> GetOrdersByPeriod(DateTime startDate, DateTime endDate)
        {
            return await _storeContext.Orders
                .Include (o => o.Staff)
                .Include(o => o.OrderDetails)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .Select(order => new OrderDto
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    StaffName = order.Staff.Name,
                    TotalPrice = order.OrderDetails.Sum(od => od.Quantity * od.UnitPrice)
                })
                .ToListAsync();
        }

        public async Task<List<OrderDto>> GetOrdersByPeriod(DateTime startDate, DateTime endDate, string staffName)
        {
            var staff = _storeContext.Staffs.FirstOrDefault(s => s.Name == staffName);
            return await _storeContext.Orders
                .Include(o => o.Staff)
                .Include(o => o.OrderDetails)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.StaffId == staff.StaffId)
                .Select(order => new OrderDto
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    StaffName = order.Staff.Name,
                    TotalPrice = order.OrderDetails.Sum(od => od.Quantity * od.UnitPrice)
                })
                .ToListAsync();
        }

        public async Task<List<OrderDto>> GetOrdersByOrderDate(DateTime orderDate, string staffName)
        {
            var staff = _storeContext.Staffs.FirstOrDefault(s => s.Name == staffName);
            var orders = await _storeContext.Orders
                .Include(o => o.Staff)
                .Include(o => o.OrderDetails)
                .Where(o => o.OrderDate.Date == orderDate.Date)
                .Select(order => new OrderDto
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    StaffName = order.Staff.Name,
                    TotalPrice = order.OrderDetails.Sum(od => od.Quantity * od.UnitPrice)
                })
                .ToListAsync();
            return orders;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _storeContext.Products.ToListAsync();
        }

        public async Task<Product> GetProductByNameAsync(string productName)
        {
            return await _storeContext.Products.FirstOrDefaultAsync(p => p.ProductName == productName);
        }




        public class OrderDto
        {
            public int OrderId { get; set; }
            public DateTime OrderDate { get; set; }
            public DateTime EndDate { get; set; }
            public string StaffName { get; set; } = string.Empty;
            public decimal TotalPrice { get; set; }
        }

        public class OrderDetailDto
        {
            public int OrderDetailId { get; set; }
            public int OrderId { get; set; }
            public int ProductId { get; set; }
            public string ProductName { get; set; } = string.Empty;
            public int Quantity { get; set; }
            public int UnitPrice { get; set; }
            public decimal TotalPrice { get; set; }
        }

    }
}
