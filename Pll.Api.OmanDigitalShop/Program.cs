// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// API Program.cs: نقطة البداية للـ REST API
// يحتوي على جميع إعدادات الخدمات والـ Middleware
// ============================================

using BLL.OmanDigitalShop.Context;
using BLL.OmanDigitalShop.Repositories;
using DAL.OmanDigitalShop.Interfaces;
using DAL.OmanDigitalShop.Models.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SLL.OmanDigitalShop.Interfaces;
using SLL.OmanDigitalShop.Services;
using System.Text;

namespace Pll.Api.OmanDigitalShop;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ============================================
        // إعداد الكنترولرز
        // ============================================

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                // تجاهل الحلقات في العلاقات
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

        // ============================================
        // إعداد Swagger/OpenAPI
        // ============================================

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Oman Digital Shop API",
                Version = "v1",
                Description = "REST API for Oman Digital Shop E-commerce Platform"
            });

            // إضافة دعم JWT في Swagger
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "أدخل التوكن بهذا الشكل: Bearer {token}"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        // ============================================
        // إعداد الداتابيز - Database Configuration
        // ============================================

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConn")));

        // ============================================
        // إعداد Identity للمصادقة - Identity Configuration
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
        // إعداد JWT Authentication
        // ============================================

        var jwtSecret = builder.Configuration["JWT:Secret"] ?? "OmanDigitalShopSecretKey2024!@#$%^&*()";
        var jwtIssuer = builder.Configuration["JWT:Issuer"] ?? "OmanDigitalShop";
        var jwtAudience = builder.Configuration["JWT:Audience"] ?? "OmanDigitalShopUsers";

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
            };
        });

        // ============================================
        // إعداد CORS للسماح للـ Angular Frontend
        // ============================================

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAngular", policy =>
            {
                policy.WithOrigins(
                        "http://localhost:4200",  // Angular default port
                        "http://localhost:3000",  // Alternative port
                        "https://localhost:4200"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });

            // للتطوير - السماح لأي مصدر
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        // ============================================
        // تسجيل الريبوزيتوريز - Repository Registration
        // ============================================

        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();

        // ============================================
        // تسجيل الخدمات - Service Registration
        // ============================================

        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IOrderService, OrderService>();

        // ============================================
        // بناء التطبيق
        // ============================================

        var app = builder.Build();

        // ============================================
        // إنشاء الأدوار الافتراضية
        // ============================================

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Admin", "Customer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        // ============================================
        // إعداد الـ Middleware Pipeline
        // ============================================
        // الترتيب مهم جداً: CORS → Authentication → Authorization → Controllers

        // تفعيل Swagger في بيئة التطوير
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Oman Digital Shop API v1");
                options.RoutePrefix = "swagger";
            });
        }

        // إعادة التوجيه لـ HTTPS
        app.UseHttpsRedirection();

        // تفعيل CORS (يجب أن يكون قبل Authentication)
        app.UseCors("AllowAngular");

        // تفعيل المصادقة والتفويض
        app.UseAuthentication();
        app.UseAuthorization();

        // تعيين الكنترولرز
        app.MapControllers();

        // تشغيل التطبيق
        app.Run();
    }
}
