using Estore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public class OrderDto
        {
            public int OrderId { get; set; }
            public DateTime OrderDate { get; set; }
            public DateTime EndDate { get; set; }
            public string StaffName { get; set; }
            public decimal TotalPrice { get; set; }
        }



    }
}
