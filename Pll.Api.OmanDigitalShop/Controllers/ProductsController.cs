// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// ProductsController: كنترولر المنتجات للـ API
// يوفر جميع عمليات CRUD للمنتجات
// ============================================

using DAL.OmanDigitalShop.Models.Products;
using Microsoft.AspNetCore.Mvc;
using SLL.OmanDigitalShop.Interfaces;

namespace Pll.Api.OmanDigitalShop.Controllers
{
    /// <summary>
    /// كنترولر المنتجات - يوفر REST API للتعامل مع المنتجات
    /// </summary>
    public class ProductsController : BaseController
    {
        // ============================================
        // المتغيرات الخاصة
        // ============================================

        private readonly IProductService _productService;

        // ============================================
        // Constructor - حقن التبعيات
        // ============================================

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // ============================================
        // عمليات القراءة - GET Endpoints
        // ============================================

        /// <summary>
        /// الحصول على جميع المنتجات مع الفئات
        /// GET: api/products
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// الحصول على المنتجات النشطة فقط
        /// GET: api/products/active
        /// </summary>
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Product>>> GetActiveProducts()
        {
            var products = await _productService.GetActiveProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// الحصول على منتج بالمعرف
        /// GET: api/products/5
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound(new { message = "المنتج غير موجود", productId = id });
            }

            return Ok(product);
        }

        /// <summary>
        /// الحصول على منتجات فئة معينة
        /// GET: api/products/category/5
        /// </summary>
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            return Ok(products);
        }

        /// <summary>
        /// البحث في المنتجات
        /// GET: api/products/search?term=laptop
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProducts([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return BadRequest(new { message = "يرجى إدخال كلمة للبحث" });
            }

            var products = await _productService.SearchProductsAsync(term);
            return Ok(products);
        }

        // ============================================
        // عمليات الإنشاء - POST Endpoints
        // ============================================

        /// <summary>
        /// إنشاء منتج جديد
        /// POST: api/products
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdProduct = await _productService.CreateProductAsync(product);

            // إرجاع 201 Created مع رابط المنتج الجديد
            return CreatedAtAction(
                nameof(GetProduct),
                new { id = createdProduct.Id },
                createdProduct
            );
        }

        // ============================================
        // عمليات التحديث - PUT Endpoints
        // ============================================

        /// <summary>
        /// تحديث منتج
        /// PUT: api/products/5
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            // التحقق من تطابق المعرف
            if (id != product.Id)
            {
                return BadRequest(new { message = "المعرف غير متطابق" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // التحقق من وجود المنتج
            if (!await _productService.ProductExistsAsync(id))
            {
                return NotFound(new { message = "المنتج غير موجود", productId = id });
            }

            await _productService.UpdateProductAsync(product);

            return NoContent(); // 204 No Content
        }

        // ============================================
        // عمليات الحذف - DELETE Endpoints
        // ============================================

        /// <summary>
        /// حذف منتج
        /// DELETE: api/products/5
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            // التحقق من وجود المنتج
            if (!await _productService.ProductExistsAsync(id))
            {
                return NotFound(new { message = "المنتج غير موجود", productId = id });
            }

            await _productService.DeleteProductAsync(id);

            return NoContent(); // 204 No Content
        }

        // ============================================
        // عمليات مساعدة - Utility Endpoints
        // ============================================

        /// <summary>
        /// الحصول على عدد المنتجات
        /// GET: api/products/count
        /// </summary>
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetProductsCount()
        {
            var count = await _productService.GetProductsCountAsync();
            return Ok(new { count });
        }
    }
}
