// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// OrderRepository: ريبوزيتوري الطلبات
// يرث من GenericRepository ويضيف عمليات خاصة بالطلبات
// ============================================

using BLL.OmanDigitalShop.Context;
using DAL.OmanDigitalShop.Interfaces;
using DAL.OmanDigitalShop.Models.Orders;
using Microsoft.EntityFrameworkCore;

namespace BLL.OmanDigitalShop.Repositories
{
    /// <summary>
    /// ريبوزيتوري الطلبات
    /// يحتوي على جميع عمليات الطلبات
    /// </summary>
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        // ============================================
        // Constructor
        // ============================================

        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        // ============================================
        // عمليات خاصة بالطلبات
        // ============================================

        /// <summary>
        /// الحصول على طلب مع جميع التفاصيل
        /// يشمل عناصر الطلب والمنتجات والمستخدم
        /// </summary>
        public async Task<Order?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(o => o.AppUser) // المستخدم صاحب الطلب
                .Include(o => o.OrderItems) // عناصر الطلب
                    .ThenInclude(oi => oi.Product) // المنتج في كل عنصر
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        /// <summary>
        /// الحصول على جميع الطلبات مع التفاصيل
        /// </summary>
        public async Task<IEnumerable<Order>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(o => o.AppUser)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate) // الأحدث أولاً
                .ToListAsync();
        }

        /// <summary>
        /// الحصول على طلبات مستخدم معين
        /// </summary>
        public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId)
        {
            return await _dbSet
                .Where(o => o.AppUserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        /// <summary>
        /// الحصول على الطلبات بحالة معينة
        /// مثال: جميع الطلبات المعلقة
        /// </summary>
        public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status)
        {
            return await _dbSet
                .Where(o => o.Status == status)
                .Include(o => o.AppUser)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        /// <summary>
        /// تحديث حالة الطلب
        /// </summary>
        public async Task UpdateStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await GetByIdAsync(orderId);
            if (order != null)
            {
                order.Status = newStatus;
                await UpdateAsync(order);
            }
        }
    }
}
