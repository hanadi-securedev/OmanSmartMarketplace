// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// Address Model: موديل العنوان
// يمثل عناوين التوصيل للمستخدم
// ============================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.OmanDigitalShop.Models.Users
{
    /// <summary>
    /// موديل العنوان - يحتوي على عناوين التوصيل
    /// </summary>
    public class Address : BaseEntity
    {
        // ============================================
        // تفاصيل العنوان
        // ============================================

        [Required(ErrorMessage = "العنوان مطلوب")]
        [MaxLength(200)]
        public string Street { get; set; } = string.Empty;

        [Required(ErrorMessage = "المدينة مطلوبة")]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? State { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        [MaxLength(100)]
        public string Country { get; set; } = "Oman";

        // ============================================
        // هل هذا العنوان الافتراضي؟
        // ============================================

        public bool IsDefault { get; set; } = false;

        // ============================================
        // العلاقة مع المستخدم
        // كل عنوان ينتمي لمستخدم واحد
        // ============================================

        [ForeignKey(nameof(AppUser))]
        public string AppUserId { get; set; } = string.Empty;

        public virtual AppUser? AppUser { get; set; }
    }
}
