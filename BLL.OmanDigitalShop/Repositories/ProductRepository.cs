// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// ProductRepository: ريبوزيتوري المنتجات
// يرث من GenericRepository ويضيف عمليات خاصة بالمنتجات
// ============================================

using BLL.OmanDigitalShop.Context;
using DAL.OmanDigitalShop.Interfaces;
using DAL.OmanDigitalShop.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace BLL.OmanDigitalShop.Repositories
{
    /// <summary>
    /// ريبوزيتوري المنتجات
    /// يحتوي على جميع عمليات المنتجات
    /// </summary>
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        // ============================================
        // Constructor
        // ============================================

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        // ============================================
        // عمليات خاصة بالمنتجات
        // ============================================

        /// <summary>
        /// الحصول على جميع المنتجات مع الفئات
        /// نستخدم Include لجلب الفئة مع كل منتج
        /// </summary>
        public async Task<IEnumerable<Product>> GetAllWithCategoryAsync()
        {
            return await _dbSet
                .Include(p => p.Category) // جلب الفئة المرتبطة
                .ToListAsync();
        }

        /// <summary>
        /// الحصول على منتج مع الفئة
        /// </summary>
        public async Task<Product?> GetByIdWithCategoryAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// الحصول على منتجات فئة معينة
        /// </summary>
        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
        {
            return await _dbSet
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Category)
                .ToListAsync();
        }

        /// <summary>
        /// الحصول على المنتجات النشطة فقط
        /// </summary>
        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
        {
            return await _dbSet
                .Where(p => p.IsActive)
                .Include(p => p.Category)
                .ToListAsync();
        }

        /// <summary>
        /// البحث في المنتجات بالاسم
        /// </summary>
        public async Task<IEnumerable<Product>> SearchByNameAsync(string searchTerm)
        {
            return await _dbSet
                .Where(p => p.Name.Contains(searchTerm))
                .Include(p => p.Category)
                .ToListAsync();
        }
    }
}
