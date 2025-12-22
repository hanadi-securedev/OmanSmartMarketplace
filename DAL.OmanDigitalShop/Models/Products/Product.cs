// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// Product Model: موديل المنتج
// يمثل المنتجات في المتجر الإلكتروني
// ============================================

using DAL.OmanDigitalShop.Models.Orders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.OmanDigitalShop.Models.Products
{
    /// <summary>
    /// موديل المنتج - يحتوي على جميع بيانات المنتج
    /// </summary>
    public class Product : BaseEntity
    {
        // ============================================
        // الخصائص الأساسية للمنتج
        // ============================================

        [Required(ErrorMessage = "اسم المنتج مطلوب")]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "السعر مطلوب")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // ============================================
        // خصائص المخزون
        // ============================================

        public int StockQuantity { get; set; } = 0;

        // ============================================
        // خصائص الصورة
        // ============================================

        public string? ImageUrl { get; set; }

        // ============================================
        // خصائص الحالة والتواريخ
        // ============================================

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // ============================================
        // العلاقة مع الفئة (Category)
        // كل منتج ينتمي لفئة واحدة
        // ============================================

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }

        // ============================================
        // العلاقة مع عناصر الطلب (OrderItems)
        // منتج واحد يمكن أن يكون في عدة طلبات
        // ============================================

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
