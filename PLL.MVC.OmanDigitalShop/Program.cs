// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// Program.cs: نقطة البداية للتطبيق
// هنا نقوم بتسجيل جميع الخدمات والإعدادات
// ============================================

using BLL.OmanDigitalShop.Context;
using BLL.OmanDigitalShop.Repositories;
using DAL.OmanDigitalShop.Interfaces;
using DAL.OmanDigitalShop.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SLL.OmanDigitalShop.Interfaces;
using SLL.OmanDigitalShop.Services;

namespace PLL.MVC.OmanDigitalShop;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ============================================
        // إضافة الخدمات للـ Container
        // ============================================

        // إضافة MVC
        builder.Services.AddControllersWithViews();

        // ============================================
        // إعداد الداتابيز
        // ============================================

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConn")));

        // ============================================
        // إعداد Identity للمصادقة
        // ============================================

        builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            // إعدادات كلمة المرور
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;

            // إعدادات المستخدم
            options.User.RequireUniqueEmail = true;

        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // ============================================
        // إعداد الـ Cookies للمصادقة
        // ============================================

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.ExpireTimeSpan = TimeSpan.FromDays(7);
        });

        // ============================================
        // تسجيل الريبوزيتوريز (Dependency Injection)
        // ============================================

        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();

        // ============================================
        // تسجيل الخدمات (Services)
        // ============================================

        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IOrderService, OrderService>();

        // ============================================
        // بناء التطبيق
        // ============================================

        var app = builder.Build();

        // ============================================
        // تشغيل Seed للـ Roles والـ Admin
        // ============================================

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            await SeedRolesAndAdmin(services);
        }

        // ============================================
        // إعداد HTTP Request Pipeline
        // ============================================

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        // مهم: المصادقة قبل التفويض
        app.UseAuthentication();
        app.UseAuthorization();

        // ============================================
        // إعداد الـ Routes
        // ============================================

        // Route للـ Areas (Admin)
        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

        // Route الافتراضي
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }

    // ============================================
    // دالة Seed للـ Roles والأدمن
    // ============================================

    /// <summary>
    /// إنشاء الأدوار والمستخدم الأدمن الافتراضي
    /// </summary>
    private static async Task SeedRolesAndAdmin(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        // ============================================
        // إنشاء الأدوار
        // ============================================

        string[] roles = { "Admin", "Customer" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // ============================================
        // إنشاء المستخدم الأدمن
        // ============================================

        var adminEmail = "admin@omanshop.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            var admin = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Admin",
                LastName = "User",
                EmailConfirmed = true,
                CreatedAt = DateTime.Now
            };

            // كلمة المرور: Admin@123
            var result = await userManager.CreateAsync(admin, "Admin@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
