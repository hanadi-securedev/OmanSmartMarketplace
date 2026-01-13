// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// AuthResponseDto: نموذج استجابة المصادقة
// يُرجع بيانات المستخدم مع التوكن
// ============================================

namespace Pll.Api.OmanDigitalShop.DTOs.Auth
{
    /// <summary>
    /// نموذج استجابة تسجيل الدخول/التسجيل
    /// </summary>
    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
        public UserDto? User { get; set; }
    }

    /// <summary>
    /// نموذج بيانات المستخدم
    /// </summary>
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
