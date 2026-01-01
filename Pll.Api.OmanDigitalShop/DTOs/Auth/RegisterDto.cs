// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// RegisterDto: نموذج التسجيل للـ API
// يستخدم لاستلام بيانات التسجيل
// ============================================

using System.ComponentModel.DataAnnotations;

namespace Pll.Api.OmanDigitalShop.DTOs.Auth
{
    /// <summary>
    /// نموذج التسجيل
    /// </summary>
    public class RegisterDto
    {
        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "الاسم الأخير مطلوب")]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [Compare("Password", ErrorMessage = "كلمة المرور غير متطابقة")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
