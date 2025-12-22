// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// AccountController: كونترولر الحسابات
// يتحكم في عمليات التسجيل وتسجيل الدخول والخروج
// ============================================

using DAL.OmanDigitalShop.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PLL.MVC.OmanDigitalShop.ViewModels.Account;

namespace PLL.MVC.OmanDigitalShop.Controllers
{
    /// <summary>
    /// كونترولر الحسابات - يدير عمليات المصادقة
    /// </summary>
    public class AccountController : Controller
    {
        // ============================================
        // المتغيرات الخاصة
        // ============================================

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        // ============================================
        // Constructor - حقن التبعيات
        // ============================================

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ============================================
        // تسجيل الدخول - Login
        // ============================================

        /// <summary>
        /// عرض صفحة تسجيل الدخول
        /// </summary>
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// معالجة تسجيل الدخول
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                // محاولة تسجيل الدخول باستخدام البريد الإلكتروني
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // التحقق من أن المستخدم أدمن
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        // توجيه الأدمن للداشبورد
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    }

                    // توجيه المستخدم العادي
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "بيانات الدخول غير صحيحة");
            }

            return View(model);
        }

        // ============================================
        // التسجيل - Register
        // ============================================

        /// <summary>
        /// عرض صفحة التسجيل
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// معالجة التسجيل
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // إنشاء مستخدم جديد
                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    CreatedAt = DateTime.Now
                };

                // محاولة إنشاء المستخدم
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // إضافة المستخدم لدور Customer
                    await _userManager.AddToRoleAsync(user, "Customer");

                    // تسجيل الدخول تلقائياً
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }

                // إضافة الأخطاء للعرض
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // ============================================
        // تسجيل الخروج - Logout
        // ============================================

        /// <summary>
        /// تسجيل الخروج
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // ============================================
        // رفض الوصول - Access Denied
        // ============================================

        /// <summary>
        /// صفحة رفض الوصول
        /// </summary>
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
