// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// AppUser Model: موديل المستخدم
// يرث من IdentityUser ويضيف خصائص إضافية
// ============================================

using DAL.OmanDigitalShop.Models.Orders;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DAL.OmanDigitalShop.Models.Users
{
    /// <summary>
    /// موديل المستخدم - يمثل مستخدم النظام (عميل أو أدمن)
    /// يرث من IdentityUser للحصول على خصائص الهوية الجاهزة
    /// </summary>
    public class AppUser : IdentityUser
    {
        // ============================================
        // معلومات المستخدم الشخصية
        // ============================================

        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "الاسم الأخير مطلوب")]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        // ============================================
        // تاريخ الإنشاء
        // ============================================

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // ============================================
        // خاصية محسوبة للاسم الكامل
        // ============================================

        public string FullName => $"{FirstName} {LastName}";

        // ============================================
        // العلاقة مع العناوين
        // مستخدم واحد يمكن أن يكون له عدة عناوين
        // ============================================

        public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

        // ============================================
        // العلاقة مع الطلبات
        // مستخدم واحد يمكن أن يكون له عدة طلبات
        // ============================================

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
