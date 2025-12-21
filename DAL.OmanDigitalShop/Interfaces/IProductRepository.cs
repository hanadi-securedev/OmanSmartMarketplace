using DAL.OmanDigitalShop.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.OmanDigitalShop.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(); // Returns a list of all products
        Task<Product> GetProductByIdAsync(int productId); // Returns a product by its ID
        Task AddProductAsync(Product product);  // Adds a new product
        Task UpdateProductAsync(Product product); // Updates an existing product
        Task DeleteProductAsync(int productId); // Deletes a product by its ID
    }
}
