// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// Category Model: موديل الفئة/التصنيف
// يمثل تصنيفات المنتجات (ملابس، إلكترونيات، إلخ)
// ============================================

using System.ComponentModel.DataAnnotations;

namespace DAL.OmanDigitalShop.Models.Products
{
    /// <summary>
    /// موديل الفئة - يحتوي على تصنيفات المنتجات
    /// </summary>
    public class Category : BaseEntity
    {
        // ============================================
        // الخصائص الأساسية للفئة
        // ============================================

        [Required(ErrorMessage = "اسم الفئة مطلوب")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        // ============================================
        // صورة الفئة (اختياري)
        // ============================================

        public string? ImageUrl { get; set; }

        // ============================================
        // حالة الفئة
        // ============================================

        public bool IsActive { get; set; } = true;

        // ============================================
        // العلاقة مع المنتجات
        // فئة واحدة يمكن أن تحتوي على عدة منتجات
        // One-to-Many Relationship
        // ============================================

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
