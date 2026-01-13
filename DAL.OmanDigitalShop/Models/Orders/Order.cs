// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// Order Model: موديل الطلب
// يمثل طلب العميل في المتجر
// ============================================

using DAL.OmanDigitalShop.Models.Orders.enums;
using DAL.OmanDigitalShop.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.OmanDigitalShop.Models.Orders
{
    /// <summary>
    /// موديل الطلب - يحتوي على بيانات طلب العميل
    /// </summary>
    public class Order : BaseEntity
    {
        // ============================================
        // معلومات الطلب الأساسية
        // ============================================

        // تاريخ إنشاء الطلب
        public DateTime OrderDate { get; set; } = DateTime.Now;

        // حالة الطلب (Pending, Confirmed, Shipping, Delivered, Cancelled)
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // ============================================
        // معلومات التوصيل
        // ============================================

        [Required(ErrorMessage = "عنوان التوصيل مطلوب")]
        [MaxLength(500)]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "المدينة مطلوبة")]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        // ============================================
        // معلومات الدفع
        // ============================================

        // المجموع الفرعي (قبل الخصم)
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        // قيمة الخصم
        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; } = 0;

        // المجموع الكلي (بعد الخصم)
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        // ============================================
        // العلاقة مع المستخدم (AppUser)
        // كل طلب ينتمي لمستخدم واحد
        // ============================================

        [ForeignKey(nameof(AppUser))]
        public string AppUserId { get; set; } = string.Empty;

        public virtual AppUser? AppUser { get; set; }

        // ============================================
        // العلاقة مع عناصر الطلب (OrderItems)
        // طلب واحد يمكن أن يحتوي على عدة عناصر
        // ============================================

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
