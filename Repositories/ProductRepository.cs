using Estore.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace Estore.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyStoreContext _storeContext;
        public ProductRepository(MyStoreContext storeContext)
        {
            _storeContext = storeContext;
        }
        public async Task<IEnumerable<Category>> GetCategories()
        {
            try
            {
                return await _storeContext.Categories.ToArrayAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving categories: {ex.Message}");
                return Enumerable.Empty<Category>();
            }
        }

        public async Task<IEnumerable<Product>> GetProducts(string keyword, IEnumerable<int> categoryIds)
        {
            try
            {
                var products = await _storeContext.Products.ToArrayAsync();
                if (!string.IsNullOrEmpty(keyword))
                {
                    products = products.Where(o => o.ProductName.Contains(keyword)).ToArray();
                }
                if (categoryIds != null && categoryIds.Any())
                {
                    products = products.Where(o => categoryIds.Contains(o.CategoryId)).ToArray();
                }
                return products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving products: {ex.Message}");
                return Enumerable.Empty<Product>();
            }
        }

        public async Task<Product> GetProductByID(int productId)
        {
            try
            {
                var product = await _storeContext.Products.FirstOrDefaultAsync(o => o.ProductId == productId);
                if (product == null)
                {
                    MessageBox.Show($"Product with id {productId} does not exist!");
                }
                return product;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving product by id: {ex.Message}");
                return null;
            }
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
