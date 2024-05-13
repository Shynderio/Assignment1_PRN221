using Estore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyStoreContext _storeContext;
        public ProductRepository(MyStoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _storeContext.Products.ToList();
        }

        public Product GetProductByID(int productId)
        {
            return _storeContext.Products.FirstOrDefault(p => p.ProductId == productId)!;
        }

        public void InsertProduct(Product product)
        {
            _storeContext.Products.Add(product);
            _storeContext.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            _storeContext.Products.Remove(product);
            _storeContext.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _storeContext.Products.Update(product);
            _storeContext.SaveChanges();
        }


    }
}
