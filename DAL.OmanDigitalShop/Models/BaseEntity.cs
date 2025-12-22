// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// BaseEntity: الكلاس الأساسي لجميع الموديلات
// كل موديل في المشروع سيرث من هذا الكلاس
// هذا يوفر علينا تكرار الـ Id في كل موديل
// ============================================

namespace DAL.OmanDigitalShop.Models
{
    /// <summary>
    /// الكلاس الأساسي - كل الموديلات ترث منه
    /// </summary>
    public abstract class BaseEntity
    {
        public int Id { get; set; }
    }
}
