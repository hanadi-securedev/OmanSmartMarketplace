// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// CategoriesController: كنترولر الفئات للـ API
// يوفر جميع عمليات CRUD للفئات
// ============================================

using DAL.OmanDigitalShop.Models.Products;
using Microsoft.AspNetCore.Mvc;
using SLL.OmanDigitalShop.Interfaces;

namespace Pll.Api.OmanDigitalShop.Controllers
{
    /// <summary>
    /// كنترولر الفئات - يوفر REST API للتعامل مع الفئات
    /// </summary>
    public class CategoriesController : BaseController
    {
        // ============================================
        // المتغيرات الخاصة
        // ============================================

        private readonly ICategoryService _categoryService;

        // ============================================
        // Constructor - حقن التبعيات
        // ============================================

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // ============================================
        // عمليات القراءة - GET Endpoints
        // ============================================

        /// <summary>
        /// الحصول على جميع الفئات
        /// GET: api/categories
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        /// <summary>
        /// الحصول على الفئات النشطة فقط
        /// GET: api/categories/active
        /// </summary>
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Category>>> GetActiveCategories()
        {
            var categories = await _categoryService.GetActiveCategoriesAsync();
            return Ok(categories);
        }

        /// <summary>
        /// الحصول على فئة بالمعرف
        /// GET: api/categories/5
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound(new { message = "الفئة غير موجودة", categoryId = id });
            }

            return Ok(category);
        }

        /// <summary>
        /// الحصول على فئة مع منتجاتها
        /// GET: api/categories/5/products
        /// </summary>
        [HttpGet("{id}/products")]
        public async Task<ActionResult<Category>> GetCategoryWithProducts(int id)
        {
            var category = await _categoryService.GetCategoryWithProductsAsync(id);

            if (category == null)
            {
                return NotFound(new { message = "الفئة غير موجودة", categoryId = id });
            }

            return Ok(category);
        }

        // ============================================
        // عمليات الإنشاء - POST Endpoints
        // ============================================

        /// <summary>
        /// إنشاء فئة جديدة
        /// POST: api/categories
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // التحقق من عدم تكرار اسم الفئة
            if (await _categoryService.CategoryNameExistsAsync(category.Name))
            {
                return Conflict(new { message = "يوجد فئة بنفس الاسم" });
            }

            var createdCategory = await _categoryService.CreateCategoryAsync(category);

            // إرجاع 201 Created مع رابط الفئة الجديدة
            return CreatedAtAction(
                nameof(GetCategory),
                new { id = createdCategory.Id },
                createdCategory
            );
        }

        // ============================================
        // عمليات التحديث - PUT Endpoints
        // ============================================

        /// <summary>
        /// تحديث فئة
        /// PUT: api/categories/5
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            // التحقق من تطابق المعرف
            if (id != category.Id)
            {
                return BadRequest(new { message = "المعرف غير متطابق" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // التحقق من وجود الفئة
            if (!await _categoryService.CategoryExistsAsync(id))
            {
                return NotFound(new { message = "الفئة غير موجودة", categoryId = id });
            }

            await _categoryService.UpdateCategoryAsync(category);

            return NoContent(); // 204 No Content
        }

        // ============================================
        // عمليات الحذف - DELETE Endpoints
        // ============================================

        /// <summary>
        /// حذف فئة
        /// DELETE: api/categories/5
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            // التحقق من وجود الفئة
            if (!await _categoryService.CategoryExistsAsync(id))
            {
                return NotFound(new { message = "الفئة غير موجودة", categoryId = id });
            }

            await _categoryService.DeleteCategoryAsync(id);

            return NoContent(); // 204 No Content
        }

        // ============================================
        // عمليات مساعدة - Utility Endpoints
        // ============================================

        /// <summary>
        /// الحصول على عدد الفئات
        /// GET: api/categories/count
        /// </summary>
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCategoriesCount()
        {
            var count = await _categoryService.GetCategoriesCountAsync();
            return Ok(new { count });
        }
    }
}
