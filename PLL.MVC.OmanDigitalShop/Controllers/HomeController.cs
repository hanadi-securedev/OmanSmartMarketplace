// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// HomeController: الكونترولر الرئيسي
// يعرض الصفحة الرئيسية للمتجر
// ============================================

using Microsoft.AspNetCore.Mvc;
using PLL.MVC.OmanDigitalShop.Models;
using SLL.OmanDigitalShop.Interfaces;
using System.Diagnostics;

namespace PLL.MVC.OmanDigitalShop.Controllers
{
    /// <summary>
    /// الكونترولر الرئيسي للموقع
    /// </summary>
    public class HomeController : Controller
    {
        // ============================================
        // المتغيرات الخاصة
        // ============================================

        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        // ============================================
        // Constructor
        // ============================================

        public HomeController(
            IProductService productService,
            ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        // ============================================
        // الصفحة الرئيسية
        // ============================================

        /// <summary>
        /// عرض الصفحة الرئيسية مع المنتجات
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetActiveProductsAsync();
            var categories = await _categoryService.GetActiveCategoriesAsync();

            ViewBag.Categories = categories;
            return View(products);
        }

        // ============================================
        // صفحة الخصوصية
        // ============================================

        public IActionResult Privacy()
        {
            return View();
        }

        // ============================================
        // صفحة الخطأ
        // ============================================

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
