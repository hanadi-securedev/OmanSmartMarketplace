// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// BaseController: الكنترولر الأساسي للـ API
// جميع الكنترولرز ترث من هذا الكنترولر
// ============================================

using Microsoft.AspNetCore.Mvc;

namespace Pll.Api.OmanDigitalShop.Controllers
{
    /// <summary>
    /// الكنترولر الأساسي - يحدد المسار الافتراضي للـ API
    /// Route: api/[controller]
    /// جميع الكنترولرز ترث منه لتحصل على:
    /// 1. المسار التلقائي api/[controller]
    /// 2. سمة ApiController للتحقق التلقائي من ModelState
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        // ============================================
        // يمكن إضافة وظائف مشتركة هنا
        // مثل: GetCurrentUserId(), HandleError()
        // ============================================
    }
}
