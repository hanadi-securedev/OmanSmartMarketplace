// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// IGenericRepository: الواجهة العامة للريبوزيتوري
// هذه الواجهة تحدد العمليات الأساسية لأي موديل
// ============================================

using DAL.OmanDigitalShop.Models;
using System.Linq.Expressions;

namespace DAL.OmanDigitalShop.Interfaces
{
    /// <summary>
    /// الواجهة العامة للريبوزيتوري
    /// T هو نوع الموديل (Product, Category, Order, etc.)
    /// يجب أن يكون T من نوع BaseEntity
    /// </summary>
    public interface IGenericRepository<T> where T : BaseEntity
    {
        // ============================================
        // عمليات القراءة (Read)
        // ============================================

        /// <summary>
        /// الحصول على جميع العناصر
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();
       

        /// <summary>
        /// الحصول على عنصر بواسطة المعرف
        /// </summary>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// البحث باستخدام شرط معين
        /// مثال: x => x.IsActive == true
        /// </summary>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // ============================================
        // عمليات الإضافة (Create)
        // ============================================

        /// <summary>
        /// إضافة عنصر جديد
        /// </summary>
        Task<T> AddAsync(T entity);

        // ============================================
        // عمليات التحديث (Update)
        // ============================================

        /// <summary>
        /// تحديث عنصر موجود
        /// </summary>
        Task UpdateAsync(T entity);

        // ============================================
        // عمليات الحذف (Delete)
        // ============================================

        /// <summary>
        /// حذف عنصر بواسطة المعرف
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// حذف عنصر
        /// </summary>
        Task DeleteAsync(T entity);

        // ============================================
        // عمليات مساعدة
        // ============================================

        /// <summary>
        /// التحقق من وجود عنصر بشرط معين
        /// </summary>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// الحصول على عدد العناصر
        /// </summary>
        Task<int> CountAsync();
    }
}
