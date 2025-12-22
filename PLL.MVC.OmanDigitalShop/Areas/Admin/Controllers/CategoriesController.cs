// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// CategoriesController: كونترولر إدارة الفئات
// CRUD للفئات في لوحة تحكم الأدمن
// ============================================

using DAL.OmanDigitalShop.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SLL.OmanDigitalShop.Interfaces;

namespace PLL.MVC.OmanDigitalShop.Areas.Admin.Controllers
{
    /// <summary>
    /// كونترولر إدارة الفئات للأدمن
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        // ============================================
        // المتغيرات الخاصة
        // ============================================

        private readonly ICategoryService _categoryService;

        // ============================================
        // Constructor
        // ============================================

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // ============================================
        // عرض جميع الفئات
        // ============================================

        /// <summary>
        /// عرض قائمة الفئات
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories);
        }

        // ============================================
        // عرض تفاصيل فئة
        // ============================================

        /// <summary>
        /// عرض تفاصيل فئة معينة مع منتجاتها
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryService.GetCategoryWithProductsAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // ============================================
        // إنشاء فئة جديدة
        // ============================================

        /// <summary>
        /// عرض صفحة إنشاء فئة جديدة
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// معالجة إنشاء فئة جديدة
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            // التحقق من عدم تكرار اسم الفئة
            if (await _categoryService.CategoryNameExistsAsync(category.Name))
            {
                ModelState.AddModelError("Name", "هذا الاسم موجود مسبقاً");
            }

            if (ModelState.IsValid)
            {
                await _categoryService.CreateCategoryAsync(category);
                TempData["Success"] = "تم إنشاء الفئة بنجاح";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // ============================================
        // تعديل فئة
        // ============================================

        /// <summary>
        /// عرض صفحة تعديل فئة
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        /// <summary>
        /// معالجة تعديل فئة
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _categoryService.UpdateCategoryAsync(category);
                TempData["Success"] = "تم تحديث الفئة بنجاح";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // ============================================
        // حذف فئة
        // ============================================

        /// <summary>
        /// عرض صفحة تأكيد الحذف
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        /// <summary>
        /// تأكيد حذف الفئة
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            TempData["Success"] = "تم حذف الفئة بنجاح";
            return RedirectToAction(nameof(Index));
        }
    }
}
