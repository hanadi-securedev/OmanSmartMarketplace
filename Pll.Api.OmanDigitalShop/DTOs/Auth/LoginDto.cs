// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// LoginDto: نموذج تسجيل الدخول للـ API
// يستخدم لاستلام بيانات الدخول
// ============================================

using System.ComponentModel.DataAnnotations;

namespace Pll.Api.OmanDigitalShop.DTOs.Auth
{
    /// <summary>
    /// نموذج تسجيل الدخول
    /// </summary>
    public class LoginDto
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        public string Password { get; set; } = string.Empty;
    }
}
