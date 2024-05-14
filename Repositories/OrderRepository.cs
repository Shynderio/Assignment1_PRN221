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

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _storeContext.Orders
                .Include(o => o.Staff)
                .Include(o => o.OrderDetails)
                .ToListAsync();
        }

    }
}
