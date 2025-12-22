// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// IOrderRepository: واجهة ريبوزيتوري الطلبات
// ترث من IGenericRepository وتضيف عمليات خاصة بالطلبات
// ============================================

using DAL.OmanDigitalShop.Models.Orders;

namespace DAL.OmanDigitalShop.Interfaces
{
    /// <summary>
    /// واجهة ريبوزيتوري الطلبات
    /// تحتوي على عمليات إضافية خاصة بالطلبات
    /// </summary>
    public interface IOrderRepository : IGenericRepository<Order>
    {
        // ============================================
        // عمليات خاصة بالطلبات
        // ============================================

        /// <summary>
        /// الحصول على طلب مع جميع التفاصيل
        /// </summary>
        Task<Order?> GetByIdWithDetailsAsync(int id);

        /// <summary>
        /// الحصول على جميع الطلبات مع التفاصيل
        /// </summary>
        Task<IEnumerable<Order>> GetAllWithDetailsAsync();

        /// <summary>
        /// الحصول على طلبات مستخدم معين
        /// </summary>
        Task<IEnumerable<Order>> GetByUserIdAsync(string userId);

        /// <summary>
        /// الحصول على الطلبات بحالة معينة
        /// </summary>
        Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status);

        /// <summary>
        /// تحديث حالة الطلب
        /// </summary>
        Task UpdateStatusAsync(int orderId, OrderStatus newStatus);
    }
}
