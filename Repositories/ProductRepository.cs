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
            return await _storeContext.Categories.ToArrayAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _storeContext.Products.Include(o => o.Category).OrderBy(o => o.ProductId).ToArrayAsync();
        }

        public async Task<Product> GetProductByID(int productId)
        {
            var product = await _storeContext.Products.Include(o => o.Category).FirstOrDefaultAsync(o => o.ProductId == productId);
            if (product == null)
                throw new Exception($"Product with id {productId} does not exist!");
            return product;
        }

        public async Task<Product> UpsertProduct(Product product)
        {
            if (product == null)
                throw new Exception("Invalid product");
            if (ValidateProduct(product))
            {
                var isValidCategory = await _storeContext.Categories
                        .AnyAsync(o => o.CategoryId == product.CategoryId);
                if (!isValidCategory)
                    throw new Exception($"Please re-check the product's category");
                if (product.ProductId == 0)
                {
                    var isDuplicateProduct = await _storeContext.Products
                        .AnyAsync(o => o.ProductName.ToLower().Equals(product.ProductName.Trim().ToLower()));
                    if (isDuplicateProduct)
                        throw new Exception("Duplicate product");
                    await _storeContext.AddAsync(new Product()
                    {
                        ProductName = product.ProductName,
                        CategoryId = product.CategoryId,
                        UnitPrice = product.UnitPrice,
                    });
                    await _storeContext.SaveChangesAsync();
                    MessageBox.Show($"Add product [{product.ProductName}] successfully");
                    product.ProductId = await _storeContext.Products.MaxAsync(o => o.ProductId);
                }
                else
                {
                    var existingProduct = await _storeContext.Products
                        .FirstOrDefaultAsync(o => o.ProductId == product.ProductId);
                    if (existingProduct == null)
                        throw new Exception($"Product with id [{product.ProductId}] does not exist");
                    existingProduct.CategoryId = product.CategoryId;
                    existingProduct.ProductName = product.ProductName;
                    existingProduct.UnitPrice = product.UnitPrice;
                    _storeContext.Products.Update(existingProduct);
                    await _storeContext.SaveChangesAsync();
                    MessageBox.Show($"Update product [{product.ProductName}] successfully");
                }
            }
            return await GetProductByID(product.ProductId);
        }

        static bool ValidateProduct(Product product)
        {
            if (string.IsNullOrEmpty(product.ProductName))
                throw new Exception("Product name is a required field");
            if (product.ProductName.Trim().Length < 6)
                throw new Exception("Product name must be at least 6-character long");
            //if (!product.ProductName.All(o => char.IsLetter(o)))
            //    throw new Exception("Product name must contain letters only");
            if (product.UnitPrice <= 0)
                throw new Exception("Unit price must be bigger than 0");
            return true;
        }

        public async Task<IEnumerable<Product>> DeleteProduct(int productId)
        {
            var product = await _storeContext.Products.FirstOrDefaultAsync(o => o.ProductId == productId);
            if (product == null)
                throw new Exception($"Product with id {productId} does not exist");
            var isInOrder = await _storeContext.OrderDetails.AnyAsync(o => o.ProductId == productId);
            if (isInOrder)
                throw new Exception($"Product with id [{productId}] already had order(s) so that it cannot be deleted");
            _storeContext.Products.Remove(product);
            await _storeContext.SaveChangesAsync();
            return await GetProducts();
        }
    }
}
