// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// GenericRepository: التنفيذ العام للريبوزيتوري
// يحتوي على جميع العمليات الأساسية CRUD
// ============================================

using BLL.OmanDigitalShop.Context;
using DAL.OmanDigitalShop.Interfaces;
using DAL.OmanDigitalShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BLL.OmanDigitalShop.Repositories
{
    /// <summary>
    /// التنفيذ العام للريبوزيتوري
    /// يستخدم Generic Type لكي يعمل مع أي موديل
    /// </summary>
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        // ============================================
        // المتغيرات المحمية (يمكن الوصول إليها من الـ classes الوارثة)
        // ============================================

        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        // ============================================
        // Constructor
        // _dbset.Set<T>();
        // ============================================

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>(); // الحصول على الـ DbSet الخاص بالموديل T
        }

        // ============================================
        // عمليات القراءة (Read)
        // ============================================

        /// <summary>
        /// الحصول على جميع العناصر
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// الحصول على عنصر بواسطة المعرف
        /// </summary>
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// البحث باستخدام شرط معين
        /// مثال: repo.FindAsync(x => x.IsActive == true)
        /// </summary>
        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        // ============================================
        // عمليات الإضافة (Create)
        // ============================================

        /// <summary>
        /// إضافة عنصر جديد وحفظه في الداتابيز
        /// </summary>
        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // ============================================
        // عمليات التحديث (Update)
        // ============================================

        /// <summary>
        /// تحديث عنصر موجود
        /// </summary>
        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        // ============================================
        // عمليات الحذف (Delete)
        // ============================================

        /// <summary>
        /// حذف عنصر بواسطة المعرف
        /// </summary>
        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }

        /// <summary>
        /// حذف عنصر
        /// </summary>
        public virtual async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        // ============================================
        // عمليات مساعدة
        // ============================================

        /// <summary>
        /// التحقق من وجود عنصر بشرط معين
        /// </summary>
        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        /// <summary>
        /// الحصول على عدد العناصر
        /// </summary>
        public virtual async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }
    }
}
