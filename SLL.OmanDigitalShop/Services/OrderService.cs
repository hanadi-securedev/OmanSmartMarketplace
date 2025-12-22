// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// OrderService: خدمة الطلبات
// تحتوي على منطق الأعمال الخاص بالطلبات
// ============================================

using DAL.OmanDigitalShop.Interfaces;
using DAL.OmanDigitalShop.Models.Orders;
using SLL.OmanDigitalShop.Interfaces;

namespace SLL.OmanDigitalShop.Services
{
    /// <summary>
    /// خدمة الطلبات
    /// تستخدم الريبوزيتوري للتعامل مع الداتابيز
    /// </summary>
    public class OrderService : IOrderService
    {
        // ============================================
        // المتغيرات الخاصة
        // ============================================

        private readonly IOrderRepository _orderRepository;

        // ============================================
        // Constructor - حقن التبعيات
        // ============================================

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // ============================================
        // عمليات القراءة
        // ============================================

        /// <summary>
        /// الحصول على جميع الطلبات مع التفاصيل
        /// </summary>
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllWithDetailsAsync();
        }

        /// <summary>
        /// الحصول على طلب بالمعرف
        /// </summary>
        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdWithDetailsAsync(id);
        }

        /// <summary>
        /// الحصول على طلبات مستخدم معين
        /// </summary>
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _orderRepository.GetByUserIdAsync(userId);
        }

        /// <summary>
        /// الحصول على الطلبات بحالة معينة
        /// </summary>
        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _orderRepository.GetByStatusAsync(status);
        }

        // ============================================
        // عمليات الإنشاء والتحديث
        // ============================================

        /// <summary>
        /// إنشاء طلب جديد
        /// </summary>
        public async Task<Order> CreateOrderAsync(Order order)
        {
            // تعيين تاريخ الطلب والحالة الافتراضية
            order.OrderDate = DateTime.Now;
            order.Status = OrderStatus.Pending;

            // حساب المجموع
            order.SubTotal = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);
            order.TotalAmount = order.SubTotal - order.Discount;

            return await _orderRepository.AddAsync(order);
        }

        /// <summary>
        /// تحديث حالة الطلب
        /// </summary>
        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            await _orderRepository.UpdateStatusAsync(orderId, newStatus);
        }

        /// <summary>
        /// إلغاء طلب
        /// </summary>
        public async Task CancelOrderAsync(int orderId)
        {
            await _orderRepository.UpdateStatusAsync(orderId, OrderStatus.Cancelled);
        }

        // ============================================
        // عمليات مساعدة
        // ============================================

        /// <summary>
        /// التحقق من وجود طلب
        /// </summary>
        public async Task<bool> OrderExistsAsync(int id)
        {
            return await _orderRepository.ExistsAsync(o => o.Id == id);
        }

        /// <summary>
        /// الحصول على عدد الطلبات
        /// </summary>
        public async Task<int> GetOrdersCountAsync()
        {
            return await _orderRepository.CountAsync();
        }

        /// <summary>
        /// الحصول على عدد الطلبات المعلقة
        /// </summary>
        public async Task<int> GetPendingOrdersCountAsync()
        {
            var pendingOrders = await _orderRepository.GetByStatusAsync(OrderStatus.Pending);
            return pendingOrders.Count();
        }
    }
}
