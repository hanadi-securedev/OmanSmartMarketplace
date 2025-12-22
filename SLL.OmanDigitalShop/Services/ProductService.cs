// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// ProductService: خدمة المنتجات
// تحتوي على منطق الأعمال الخاص بالمنتجات
// ============================================

using DAL.OmanDigitalShop.Interfaces;
using DAL.OmanDigitalShop.Models.Products;
using SLL.OmanDigitalShop.Interfaces;

namespace SLL.OmanDigitalShop.Services
{
    /// <summary>
    /// خدمة المنتجات
    /// تستخدم الريبوزيتوري للتعامل مع الداتابيز
    /// </summary>
    public class ProductService : IProductService
    {
        // ============================================
        // المتغيرات الخاصة
        // ============================================

        private readonly IProductRepository _productRepository;

        // ============================================
        // Constructor - حقن التبعيات
        // ============================================

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // ============================================
        // عمليات القراءة
        // ============================================

        /// <summary>
        /// الحصول على جميع المنتجات مع الفئات
        /// </summary>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllWithCategoryAsync();
        }

        /// <summary>
        /// الحصول على المنتجات النشطة فقط
        /// </summary>
        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
        {
            return await _productRepository.GetActiveProductsAsync();
        }

        /// <summary>
        /// الحصول على منتج بالمعرف
        /// </summary>
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetByIdWithCategoryAsync(id);
        }

        /// <summary>
        /// الحصول على منتجات فئة معينة
        /// </summary>
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _productRepository.GetByCategoryIdAsync(categoryId);
        }

        /// <summary>
        /// البحث في المنتجات
        /// </summary>
        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            return await _productRepository.SearchByNameAsync(searchTerm);
        }

        // ============================================
        // عمليات الإنشاء والتحديث والحذف
        // ============================================

        /// <summary>
        /// إنشاء منتج جديد
        /// </summary>
        public async Task<Product> CreateProductAsync(Product product)
        {
            // تعيين تاريخ الإنشاء
            product.CreatedAt = DateTime.Now;
            return await _productRepository.AddAsync(product);
        }

        /// <summary>
        /// تحديث منتج
        /// </summary>
        public async Task UpdateProductAsync(Product product)
        {
            await _productRepository.UpdateAsync(product);
        }

        /// <summary>
        /// حذف منتج
        /// </summary>
        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteAsync(id);
        }

        // ============================================
        // عمليات مساعدة
        // ============================================

        /// <summary>
        /// التحقق من وجود منتج
        /// </summary>
        public async Task<bool> ProductExistsAsync(int id)
        {
            return await _productRepository.ExistsAsync(p => p.Id == id);
        }

        /// <summary>
        /// الحصول على عدد المنتجات
        /// </summary>
        public async Task<int> GetProductsCountAsync()
        {
            return await _productRepository.CountAsync();
        }
    }
}
