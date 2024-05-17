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
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e.Message}");
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
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e.Message}");
                return Enumerable.Empty<Product>();
            }
        }

        public async Task<Product> GetProductByID(int productId)
        {
            try
            {
                var product = await _storeContext.Products.FirstOrDefaultAsync(o => o.ProductId == productId);
                if (product == null)
                    throw new Exception($"Product with id {productId} does not exist!");
                return product;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e.Message}");
                return new Product();
            }
        }

        public async Task<Product> InsertProduct(Product product)
        {
            try
            {
                if (product == null)
                    throw new Exception("Invalid product");
                if (ValidateProduct(product))
                {
                    var isDuplicateProduct = await _storeContext.Products
                        .AnyAsync(o => o.ProductName.ToLower().Equals(product.ProductName.Trim().ToLower()));
                    if (isDuplicateProduct)
                        throw new Exception("Invalid product");
                    var isValidCategory = await _storeContext.Categories
                        .AnyAsync(o => o.CategoryId == product.CategoryId);
                    if (!isValidCategory)
                        throw new Exception($"Please re-check the product's category");
                    var maxId = await _storeContext.Products.Select(o => o.ProductId).MaxAsync();
                    product.ProductId = maxId + 1;
                    await _storeContext.AddAsync(product);
                    await _storeContext.SaveChangesAsync();
                    MessageBox.Show($"Add product [{product.ProductName}] successfully");
                }
                return product;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e}");
                return product;
            }   
        }

        public async Task<IEnumerable<Product>> DeleteProduct(int productId)
        {
            try
            {
                var product = await _storeContext.Products.FirstOrDefaultAsync(o => o.ProductId == productId);
                if (product == null)
                    throw new Exception($"Product with id {productId} does not exist");
                _storeContext.Products.Remove(product);
                await _storeContext.SaveChangesAsync();
                return await GetProducts(string.Empty, new List<int>());
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e}");
                return await GetProducts(string.Empty, new List<int>());
            }
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            try
            {
                if (product == null)
                throw new Exception("Invalid product");
                if (ValidateProduct(product))
                {
                    var existingProduct = await _storeContext.Products
                        .FirstOrDefaultAsync(o => o.ProductId == product.ProductId);
                    if (existingProduct == null)
                        throw new Exception($"Product with id [{product.ProductId}] does not exist");
                    var isValidCategory = await _storeContext.Categories
                        .AnyAsync(o => o.CategoryId == product.CategoryId);
                    if (!isValidCategory)
                        throw new Exception($"Please re-check the product's category");
                    existingProduct.ProductName = product.ProductName;
                    existingProduct.UnitPrice = product.UnitPrice;
                    _storeContext.Products.Update(existingProduct);
                    await _storeContext.SaveChangesAsync();
                }
                return product;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e}");
                return product;
            }
        }

        static bool ValidateProduct(Product product)
        {
            if (string.IsNullOrEmpty(product.ProductName))
                throw new Exception("Product name is a required field");
            if (product.ProductName.Trim().Length < 6)
                throw new Exception("Product name must be at least 6-character long");
            if (!product.ProductName.All(o => char.IsLetter(o)))
                throw new Exception("Product name must contain letters only");
            if (product.UnitPrice <= 0)
                throw new Exception("Unit price must be bigger than 0");
            return true;
        }
    }
}
