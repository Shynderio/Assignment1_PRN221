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
        private readonly List<Order> _orders;
        public OrderRepository(MyStoreContext storeContext)
        {
            _storeContext = storeContext;

            _orders = new List<Order>
            {
                new Order
                {
                    OrderId = 1, OrderDate = DateTime.Now.AddDays(-1), StaffId = 1,
                    Staff = new Staff { StaffId = 1, Name = "John Doe" },
                    OrderDetails = new List<OrderDetail>
                    {
                        new OrderDetail { OrderDetailId = 1, OrderId = 1, ProductId = 1, Quantity = 2, UnitPrice = 100 },
                        new OrderDetail { OrderDetailId = 2, OrderId = 1, ProductId = 2, Quantity = 1, UnitPrice = 200 }
                    }
                },
                new Order
                {
                    OrderId = 2, OrderDate = DateTime.Now.AddDays(-2), StaffId = 2,
                    Staff = new Staff { StaffId = 2, Name = "Jane Doe" },
                    OrderDetails = new List<OrderDetail>
                    {
                        new OrderDetail { OrderDetailId = 3, OrderId = 2, ProductId = 1, Quantity = 3, UnitPrice = 100 },
                        new OrderDetail { OrderDetailId = 4, OrderId = 2, ProductId = 3, Quantity = 2, UnitPrice = 150 }
                    }
                }
            };
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _storeContext.Orders
                .Include(o => o.Staff)
                .Include(o => o.OrderDetails)
                .ToListAsync();
        }

    }
}
