﻿using Estore.Models;

namespace Estore.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<IEnumerable<Product>> GetProducts(string keyword, IEnumerable<int> categoryIds);
        Task<Product> GetProductByID(int productId);
        Task<Product> InsertProduct(Product product);
        Task<IEnumerable<Product>> DeleteProduct(int productId);
        Task<Product> UpdateProduct(Product product);
    }
}
