// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// ICategoryService: واجهة خدمة الفئات
// تحدد العمليات المتاحة للتعامل مع الفئات
// ============================================

using DAL.OmanDigitalShop.Models.Products;

namespace SLL.OmanDigitalShop.Interfaces
{
    /// <summary>
    /// واجهة خدمة الفئات
    /// </summary>
    public interface ICategoryService
    {
        // ============================================
        // عمليات القراءة
        // ============================================

        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category?> GetCategoryWithProductsAsync(int id);

        // ============================================
        // عمليات الإنشاء والتحديث والحذف
        // ============================================

        Task<Category> CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);

        // ============================================
        // عمليات مساعدة
        // ============================================

        Task<bool> CategoryExistsAsync(int id);
        Task<bool> CategoryNameExistsAsync(string name);
        Task<int> GetCategoriesCountAsync();
    }
}
