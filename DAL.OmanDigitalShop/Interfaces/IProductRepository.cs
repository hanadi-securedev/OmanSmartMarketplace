// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// IProductRepository: واجهة ريبوزيتوري المنتجات
// ترث من IGenericRepository وتضيف عمليات خاصة بالمنتجات
// ============================================

using DAL.OmanDigitalShop.Models.Products;

namespace DAL.OmanDigitalShop.Interfaces
{
    /// <summary>
    /// واجهة ريبوزيتوري المنتجات
    /// تحتوي على عمليات إضافية خاصة بالمنتجات
    /// </summary>
    public interface IProductRepository : IGenericRepository<Product>
    {
        // ============================================
        // عمليات خاصة بالمنتجات
        // ============================================

        /// <summary>
        /// الحصول على جميع المنتجات مع الفئات
        /// </summary>
        Task<IEnumerable<Product>> GetAllWithCategoryAsync();

        /// <summary>
        /// الحصول على منتج مع الفئة
        /// </summary>
        Task<Product?> GetByIdWithCategoryAsync(int id);

        /// <summary>
        /// الحصول على منتجات فئة معينة
        /// </summary>
        Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);

        /// <summary>
        /// الحصول على المنتجات النشطة فقط
        /// </summary>
        Task<IEnumerable<Product>> GetActiveProductsAsync();

        /// <summary>
        /// البحث في المنتجات بالاسم
        /// </summary>
        Task<IEnumerable<Product>> SearchByNameAsync(string searchTerm);
    }
}
