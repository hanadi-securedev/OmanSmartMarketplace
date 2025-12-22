// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// ICategoryRepository: واجهة ريبوزيتوري الفئات
// ترث من IGenericRepository وتضيف عمليات خاصة بالفئات
// ============================================

using DAL.OmanDigitalShop.Models.Products;

namespace DAL.OmanDigitalShop.Interfaces
{
    /// <summary>
    /// واجهة ريبوزيتوري الفئات
    /// تحتوي على عمليات إضافية خاصة بالفئات
    /// </summary>
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        // ============================================
        // عمليات خاصة بالفئات
        // ============================================

        /// <summary>
        /// الحصول على فئة مع منتجاتها
        /// </summary>
        Task<Category?> GetByIdWithProductsAsync(int id);

        /// <summary>
        /// الحصول على جميع الفئات مع عدد المنتجات
        /// </summary>
        Task<IEnumerable<Category>> GetAllWithProductCountAsync();

        /// <summary>
        /// الحصول على الفئات النشطة فقط
        /// </summary>
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();

        /// <summary>
        /// التحقق من وجود فئة بنفس الاسم
        /// </summary>
        Task<bool> ExistsByNameAsync(string name);
    }
}
