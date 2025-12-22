// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// CategoryService: خدمة الفئات
// تحتوي على منطق الأعمال الخاص بالفئات
// ============================================

using DAL.OmanDigitalShop.Interfaces;
using DAL.OmanDigitalShop.Models.Products;
using SLL.OmanDigitalShop.Interfaces;

namespace SLL.OmanDigitalShop.Services
{
    /// <summary>
    /// خدمة الفئات
    /// تستخدم الريبوزيتوري للتعامل مع الداتابيز
    /// </summary>
    public class CategoryService : ICategoryService
    {
        // ============================================
        // المتغيرات الخاصة
        // ============================================

        private readonly ICategoryRepository _categoryRepository;

        // ============================================
        // Constructor - حقن التبعيات
        // ============================================

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // ============================================
        // عمليات القراءة
        // ============================================

        /// <summary>
        /// الحصول على جميع الفئات
        /// </summary>
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        /// <summary>
        /// الحصول على الفئات النشطة فقط
        /// </summary>
        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _categoryRepository.GetActiveCategoriesAsync();
        }

        /// <summary>
        /// الحصول على فئة بالمعرف
        /// </summary>
        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// الحصول على فئة مع منتجاتها
        /// </summary>
        public async Task<Category?> GetCategoryWithProductsAsync(int id)
        {
            return await _categoryRepository.GetByIdWithProductsAsync(id);
        }

        // ============================================
        // عمليات الإنشاء والتحديث والحذف
        // ============================================

        /// <summary>
        /// إنشاء فئة جديدة
        /// </summary>
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            return await _categoryRepository.AddAsync(category);
        }

        /// <summary>
        /// تحديث فئة
        /// </summary>
        public async Task UpdateCategoryAsync(Category category)
        {
            await _categoryRepository.UpdateAsync(category);
        }

        /// <summary>
        /// حذف فئة
        /// </summary>
        public async Task DeleteCategoryAsync(int id)
        {
            await _categoryRepository.DeleteAsync(id);
        }

        // ============================================
        // عمليات مساعدة
        // ============================================

        /// <summary>
        /// التحقق من وجود فئة
        /// </summary>
        public async Task<bool> CategoryExistsAsync(int id)
        {
            return await _categoryRepository.ExistsAsync(c => c.Id == id);
        }

        /// <summary>
        /// التحقق من وجود فئة بنفس الاسم
        /// </summary>
        public async Task<bool> CategoryNameExistsAsync(string name)
        {
            return await _categoryRepository.ExistsByNameAsync(name);
        }

        /// <summary>
        /// الحصول على عدد الفئات
        /// </summary>
        public async Task<int> GetCategoriesCountAsync()
        {
            return await _categoryRepository.CountAsync();
        }
    }
}
