// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// OrdersController: كنترولر الطلبات للـ API
// يوفر جميع عمليات CRUD للطلبات
// ============================================

using DAL.OmanDigitalShop.Models.Orders;
using DAL.OmanDigitalShop.Models.Orders.enums;
using Microsoft.AspNetCore.Mvc;
using SLL.OmanDigitalShop.Interfaces;

namespace Pll.Api.OmanDigitalShop.Controllers
{
    /// <summary>
    /// كنترولر الطلبات - يوفر REST API للتعامل مع الطلبات
    /// </summary>
    public class OrdersController : BaseController
    {
        // ============================================
        // المتغيرات الخاصة
        // ============================================

        private readonly IOrderService _orderService;

        // ============================================
        // Constructor - حقن التبعيات
        // ============================================

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // ============================================
        // عمليات القراءة - GET Endpoints
        // ============================================

        /// <summary>
        /// الحصول على جميع الطلبات
        /// GET: api/orders
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        /// <summary>
        /// الحصول على طلب بالمعرف
        /// GET: api/orders/5
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound(new { message = "الطلب غير موجود", orderId = id });
            }

            return Ok(order);
        }

        /// <summary>
        /// الحصول على طلبات مستخدم معين
        /// GET: api/orders/user/abc123
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUser(string userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        /// <summary>
        /// الحصول على الطلبات بحالة معينة
        /// GET: api/orders/status/pending
        /// </summary>
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByStatus(OrderStatus status)
        {
            var orders = await _orderService.GetOrdersByStatusAsync(status);
            return Ok(orders);
        }

        /// <summary>
        /// الحصول على الطلبات المعلقة
        /// GET: api/orders/pending
        /// </summary>
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<Order>>> GetPendingOrders()
        {
            var orders = await _orderService.GetOrdersByStatusAsync(OrderStatus.Pending);
            return Ok(orders);
        }

        // ============================================
        // عمليات الإنشاء - POST Endpoints
        // ============================================

        /// <summary>
        /// إنشاء طلب جديد
        /// POST: api/orders
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // التحقق من وجود عناصر في الطلب
            if (order.OrderItems == null || !order.OrderItems.Any())
            {
                return BadRequest(new { message = "يجب إضافة منتج واحد على الأقل للطلب" });
            }

            var createdOrder = await _orderService.CreateOrderAsync(order);

            // إرجاع 201 Created مع رابط الطلب الجديد
            return CreatedAtAction(
                nameof(GetOrder),
                new { id = createdOrder.Id },
                createdOrder
            );
        }

        // ============================================
        // عمليات التحديث - PUT/PATCH Endpoints
        // ============================================

        /// <summary>
        /// تحديث حالة الطلب
        /// PUT: api/orders/5/status
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            // التحقق من وجود الطلب
            if (!await _orderService.OrderExistsAsync(id))
            {
                return NotFound(new { message = "الطلب غير موجود", orderId = id });
            }

            await _orderService.UpdateOrderStatusAsync(id, request.Status);

            return Ok(new { message = "تم تحديث حالة الطلب", orderId = id, newStatus = request.Status.ToString() });
        }

        /// <summary>
        /// تأكيد الطلب
        /// PUT: api/orders/5/confirm
        /// </summary>
        [HttpPut("{id}/confirm")]
        public async Task<IActionResult> ConfirmOrder(int id)
        {
            if (!await _orderService.OrderExistsAsync(id))
            {
                return NotFound(new { message = "الطلب غير موجود", orderId = id });
            }

            await _orderService.UpdateOrderStatusAsync(id, OrderStatus.Confirmed);

            return Ok(new { message = "تم تأكيد الطلب", orderId = id });
        }

        /// <summary>
        /// تحديث حالة الطلب إلى الشحن
        /// PUT: api/orders/5/ship
        /// </summary>
        [HttpPut("{id}/ship")]
        public async Task<IActionResult> ShipOrder(int id)
        {
            if (!await _orderService.OrderExistsAsync(id))
            {
                return NotFound(new { message = "الطلب غير موجود", orderId = id });
            }

            await _orderService.UpdateOrderStatusAsync(id, OrderStatus.Shipping);

            return Ok(new { message = "تم تحديث حالة الطلب إلى جاري الشحن", orderId = id });
        }

        /// <summary>
        /// تأكيد توصيل الطلب
        /// PUT: api/orders/5/deliver
        /// </summary>
        [HttpPut("{id}/deliver")]
        public async Task<IActionResult> DeliverOrder(int id)
        {
            if (!await _orderService.OrderExistsAsync(id))
            {
                return NotFound(new { message = "الطلب غير موجود", orderId = id });
            }

            await _orderService.UpdateOrderStatusAsync(id, OrderStatus.Delivered);

            return Ok(new { message = "تم توصيل الطلب", orderId = id });
        }

        /// <summary>
        /// إلغاء الطلب
        /// PUT: api/orders/5/cancel
        /// </summary>
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            if (!await _orderService.OrderExistsAsync(id))
            {
                return NotFound(new { message = "الطلب غير موجود", orderId = id });
            }

            await _orderService.CancelOrderAsync(id);

            return Ok(new { message = "تم إلغاء الطلب", orderId = id });
        }

        // ============================================
        // عمليات مساعدة - Utility Endpoints
        // ============================================

        /// <summary>
        /// الحصول على عدد الطلبات
        /// GET: api/orders/count
        /// </summary>
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetOrdersCount()
        {
            var count = await _orderService.GetOrdersCountAsync();
            return Ok(new { count });
        }

        /// <summary>
        /// الحصول على عدد الطلبات المعلقة
        /// GET: api/orders/pending/count
        /// </summary>
        [HttpGet("pending/count")]
        public async Task<ActionResult<int>> GetPendingOrdersCount()
        {
            var count = await _orderService.GetPendingOrdersCountAsync();
            return Ok(new { count });
        }
    }

    // ============================================
    // نماذج الطلبات (Request Models)
    // ============================================

    /// <summary>
    /// نموذج تحديث حالة الطلب
    /// </summary>
    public class UpdateStatusRequest
    {
        public OrderStatus Status { get; set; }
    }
}
