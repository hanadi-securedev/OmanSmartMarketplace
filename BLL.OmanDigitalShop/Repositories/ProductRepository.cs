using BLL.OmanDigitalShop.Context;
using DAL.OmanDigitalShop.Interfaces;
using DAL.OmanDigitalShop.Models.Products;
using Microsoft.EntityFrameworkCore;


namespace BLL.OmanDigitalShop.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task AddProductAsync(Product product)
        {
            _dbContext.products.Add(product);
            return _dbContext.SaveChangesAsync();

        }

        public Task DeleteProductAsync(int productId)
        {
            _dbContext.products.Remove(_dbContext.products.Find(productId)!);
            return _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = await _dbContext.products.ToListAsync();
            return products;


        }

        public Task<Product> GetProductByIdAsync(int productId)
        {
            var product = _dbContext.products.FindAsync(productId);
            return product.AsTask();
        }

        public Task UpdateProductAsync(Product product)
        {
            _dbContext.products.Update(product);
            return _dbContext.SaveChangesAsync();

        }
    }
}
