using Estore.Models;

namespace Estore.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProductByID(int productId);
        Task<Product> UpsertProduct(Product product);
        Task<IEnumerable<Product>> DeleteProduct(int productId);
    }
}
