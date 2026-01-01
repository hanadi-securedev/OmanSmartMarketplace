// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// AuthController: كنترولر المصادقة للـ API
// يوفر تسجيل الدخول والتسجيل باستخدام JWT
// ============================================

using DAL.OmanDigitalShop.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Pll.Api.OmanDigitalShop.DTOs.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pll.Api.OmanDigitalShop.Controllers
{
    /// <summary>
    /// كنترولر المصادقة - يوفر REST API للتسجيل وتسجيل الدخول
    /// </summary>
    public class AuthController : BaseController
    {
        // ============================================
        // المتغيرات الخاصة
        // ============================================

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;

        // ============================================
        // Constructor - حقن التبعيات
        // ============================================

        public AuthController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        // ============================================
        // تسجيل مستخدم جديد - POST Endpoint
        // ============================================

        /// <summary>
        /// تسجيل مستخدم جديد
        /// POST: api/auth/register
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "بيانات التسجيل غير صالحة"
                });
            }

            // التحقق من عدم وجود مستخدم بنفس البريد
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return Conflict(new AuthResponseDto
                {
                    Success = false,
                    Message = "البريد الإلكتروني مستخدم مسبقاً"
                });
            }

            // إنشاء مستخدم جديد
            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreatedAt = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                });
            }

            // إضافة المستخدم لدور العميل
            await _userManager.AddToRoleAsync(user, "Customer");

            // إنشاء التوكن
            var token = await GenerateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new AuthResponseDto
            {
                Success = true,
                Message = "تم التسجيل بنجاح",
                Token = token,
                Expiration = DateTime.UtcNow.AddDays(7),
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email!,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName,
                    Roles = roles
                }
            });
        }

        // ============================================
        // تسجيل الدخول - POST Endpoint
        // ============================================

        /// <summary>
        /// تسجيل الدخول
        /// POST: api/auth/login
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "بيانات الدخول غير صالحة"
                });
            }

            // البحث عن المستخدم
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized(new AuthResponseDto
                {
                    Success = false,
                    Message = "البريد الإلكتروني أو كلمة المرور غير صحيحة"
                });
            }

            // التحقق من كلمة المرور
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new AuthResponseDto
                {
                    Success = false,
                    Message = "البريد الإلكتروني أو كلمة المرور غير صحيحة"
                });
            }

            // إنشاء التوكن
            var token = await GenerateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new AuthResponseDto
            {
                Success = true,
                Message = "تم تسجيل الدخول بنجاح",
                Token = token,
                Expiration = DateTime.UtcNow.AddDays(7),
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email!,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName,
                    Roles = roles
                }
            });
        }

        // ============================================
        // الحصول على بيانات المستخدم الحالي
        // ============================================

        /// <summary>
        /// الحصول على بيانات المستخدم الحالي
        /// GET: api/auth/me
        /// </summary>
        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            // الحصول على معرف المستخدم من التوكن
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "المستخدم غير مسجل الدخول" });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "المستخدم غير موجود" });
            }

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FullName,
                Roles = roles
            });
        }

        // ============================================
        // وظائف مساعدة
        // ============================================

        /// <summary>
        /// إنشاء JWT Token
        /// </summary>
        private async Task<string> GenerateJwtToken(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // إضافة الأدوار كـ Claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["JWT:Secret"] ?? "OmanDigitalShopSecretKey2024!@#$%^&*()"));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"] ?? "OmanDigitalShop",
                audience: _configuration["JWT:Audience"] ?? "OmanDigitalShopUsers",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
