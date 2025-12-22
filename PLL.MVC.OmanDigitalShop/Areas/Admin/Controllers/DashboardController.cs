// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// DashboardController: كونترولر لوحة التحكم
// الصفحة الرئيسية للأدمن
// ============================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SLL.OmanDigitalShop.Interfaces;

namespace PLL.MVC.OmanDigitalShop.Areas.Admin.Controllers
{
    /// <summary>
    /// كونترولر لوحة التحكم للأدمن
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        // ============================================
        // المتغيرات الخاصة
        // ============================================

        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IOrderService _orderService;

        // ============================================
        // Constructor
        // ============================================

        public DashboardController(
            IProductService productService,
            ICategoryService categoryService,
            IOrderService orderService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _orderService = orderService;
        }

        // ============================================
        // الصفحة الرئيسية
        // ============================================

        /// <summary>
        /// عرض لوحة التحكم مع الإحصائيات
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // جلب الإحصائيات
            ViewBag.ProductsCount = await _productService.GetProductsCountAsync();
            ViewBag.CategoriesCount = await _categoryService.GetCategoriesCountAsync();
            ViewBag.OrdersCount = await _orderService.GetOrdersCountAsync();
            ViewBag.PendingOrdersCount = await _orderService.GetPendingOrdersCountAsync();

            return View();
        }
    }
}
