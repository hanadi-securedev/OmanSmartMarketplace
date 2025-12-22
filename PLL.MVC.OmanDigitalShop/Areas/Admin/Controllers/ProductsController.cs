// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// ProductsController: كونترولر إدارة المنتجات
// CRUD للمنتجات في لوحة تحكم الأدمن
// ============================================

using DAL.OmanDigitalShop.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SLL.OmanDigitalShop.Interfaces;

namespace PLL.MVC.OmanDigitalShop.Areas.Admin.Controllers
{
    /// <summary>
    /// كونترولر إدارة المنتجات للأدمن
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        // ============================================
        // المتغيرات الخاصة
        // ============================================

        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        // ============================================
        // Constructor
        // ============================================

        public ProductsController(
            IProductService productService,
            ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        // ============================================
        // عرض جميع المنتجات
        // ============================================

        /// <summary>
        /// عرض قائمة المنتجات
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        // ============================================
        // عرض تفاصيل منتج
        // ============================================

        /// <summary>
        /// عرض تفاصيل منتج معين
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // ============================================
        // إنشاء منتج جديد
        // ============================================

        /// <summary>
        /// عرض صفحة إنشاء منتج جديد
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // تحميل الفئات للقائمة المنسدلة
            await LoadCategoriesAsync();
            return View();
        }

        /// <summary>
        /// معالجة إنشاء منتج جديد
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.CreateProductAsync(product);
                TempData["Success"] = "تم إنشاء المنتج بنجاح";
                return RedirectToAction(nameof(Index));
            }

            await LoadCategoriesAsync();
            return View(product);
        }

        // ============================================
        // تعديل منتج
        // ============================================

        /// <summary>
        /// عرض صفحة تعديل منتج
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await LoadCategoriesAsync();
            return View(product);
        }

        /// <summary>
        /// معالجة تعديل منتج
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _productService.UpdateProductAsync(product);
                TempData["Success"] = "تم تحديث المنتج بنجاح";
                return RedirectToAction(nameof(Index));
            }

            await LoadCategoriesAsync();
            return View(product);
        }

        // ============================================
        // حذف منتج
        // ============================================

        /// <summary>
        /// عرض صفحة تأكيد الحذف
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        /// <summary>
        /// تأكيد حذف المنتج
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.DeleteProductAsync(id);
            TempData["Success"] = "تم حذف المنتج بنجاح";
            return RedirectToAction(nameof(Index));
        }

        // ============================================
        // دوال مساعدة
        // ============================================

        /// <summary>
        /// تحميل الفئات للقوائم المنسدلة
        /// </summary>
        private async Task LoadCategoriesAsync()
        {
            var categories = await _categoryService.GetActiveCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        }
    }
}
