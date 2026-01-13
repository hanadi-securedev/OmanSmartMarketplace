// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// OrdersController: كونترولر إدارة الطلبات
// إدارة الطلبات في لوحة تحكم الأدمن
// ============================================

using DAL.OmanDigitalShop.Models.Orders.enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SLL.OmanDigitalShop.Interfaces;

namespace PLL.MVC.OmanDigitalShop.Areas.Admin.Controllers
{
    /// <summary>
    /// كونترولر إدارة الطلبات للأدمن
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        // ============================================
        // المتغيرات الخاصة
        // ============================================

        private readonly IOrderService _orderService;

        // ============================================
        // Constructor
        // ============================================

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // ============================================
        // عرض جميع الطلبات
        // ============================================

        /// <summary>
        /// عرض قائمة الطلبات
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        // ============================================
        // عرض تفاصيل طلب
        // ============================================

        /// <summary>
        /// عرض تفاصيل طلب معين
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // ============================================
        // تحديث حالة الطلب
        // ============================================

        /// <summary>
        /// تحديث حالة الطلب
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
        {
            await _orderService.UpdateOrderStatusAsync(id, status);
            TempData["Success"] = "تم تحديث حالة الطلب بنجاح";
            return RedirectToAction(nameof(Details), new { id });
        }

        // ============================================
        // عرض الطلبات حسب الحالة
        // ============================================

        /// <summary>
        /// عرض الطلبات المعلقة
        /// </summary>
        public async Task<IActionResult> Pending()
        {
            var orders = await _orderService.GetOrdersByStatusAsync(OrderStatus.Pending);
            ViewBag.Title = "الطلبات المعلقة";
            return View("Index", orders);
        }

        /// <summary>
        /// عرض الطلبات المؤكدة
        /// </summary>
        public async Task<IActionResult> Confirmed()
        {
            var orders = await _orderService.GetOrdersByStatusAsync(OrderStatus.Confirmed);
            ViewBag.Title = "الطلبات المؤكدة";
            return View("Index", orders);
        }

        /// <summary>
        /// عرض الطلبات قيد الشحن
        /// </summary>
        public async Task<IActionResult> Shipping()
        {
            var orders = await _orderService.GetOrdersByStatusAsync(OrderStatus.Shipping);
            ViewBag.Title = "الطلبات قيد الشحن";
            return View("Index", orders);
        }

        /// <summary>
        /// عرض الطلبات المكتملة
        /// </summary>
        public async Task<IActionResult> Delivered()
        {
            var orders = await _orderService.GetOrdersByStatusAsync(OrderStatus.Delivered);
            ViewBag.Title = "الطلبات المكتملة";
            return View("Index", orders);
        }
    }
}
