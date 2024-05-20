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
    }
}