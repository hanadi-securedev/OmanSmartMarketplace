// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// RegisterViewModel: نموذج التسجيل
// يستخدم لاستلام بيانات التسجيل من المستخدم
// ============================================

using System.ComponentModel.DataAnnotations;

namespace PLL.MVC.OmanDigitalShop.ViewModels.Account
{
    /// <summary>
    /// نموذج التسجيل
    /// </summary>
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [MaxLength(50)]
        [Display(Name = "الاسم الأول")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "الاسم الأخير مطلوب")]
        [MaxLength(50)]
        [Display(Name = "الاسم الأخير")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمة المرور غير متطابقة")]
        [Display(Name = "تأكيد كلمة المرور")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
