// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// LoginViewModel: نموذج تسجيل الدخول
// يستخدم لاستلام بيانات الدخول من المستخدم
// ============================================

using System.ComponentModel.DataAnnotations;

namespace PLL.MVC.OmanDigitalShop.ViewModels.Account
{
    /// <summary>
    /// نموذج تسجيل الدخول
    /// </summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "تذكرني")]
        public bool RememberMe { get; set; }
    }
}
