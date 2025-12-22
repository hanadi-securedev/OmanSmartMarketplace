// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// IProductService: واجهة خدمة المنتجات
// تحدد العمليات المتاحة للتعامل مع المنتجات
// ============================================

using DAL.OmanDigitalShop.Models.Products;

namespace SLL.OmanDigitalShop.Interfaces
{
    /// <summary>
    /// واجهة خدمة المنتجات
    /// </summary>
    public interface IProductService
    {
        // ============================================
        // عمليات القراءة
        // ============================================

        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetActiveProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);

        // ============================================
        // عمليات الإنشاء والتحديث والحذف
        // ============================================

        Task<Product> CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);

        // ============================================
        // عمليات مساعدة
        // ============================================

        Task<bool> ProductExistsAsync(int id);
        Task<int> GetProductsCountAsync();
    }
}
