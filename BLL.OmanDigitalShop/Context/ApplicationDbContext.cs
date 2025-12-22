// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// ApplicationDbContext: السياق الرئيسي للداتابيز
// يرث من IdentityDbContext للحصول على جداول الهوية
// ============================================

using DAL.OmanDigitalShop.Models.Orders;
using DAL.OmanDigitalShop.Models.Products;
using DAL.OmanDigitalShop.Models.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BLL.OmanDigitalShop.Context
{
    /// <summary>
    /// السياق الرئيسي للداتابيز
    /// يحتوي على جميع الـ DbSets للموديلات
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        // ============================================
        // Constructor
        // ============================================

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ============================================
        // DbSets - جداول الداتابيز
        // ============================================

        // جدول المنتجات
        public DbSet<Product> Products { get; set; }

        // جدول الفئات
        public DbSet<Category> Categories { get; set; }

        // جدول الطلبات
        public DbSet<Order> Orders { get; set; }

        // جدول عناصر الطلبات
        public DbSet<OrderItem> OrderItems { get; set; }

        // جدول العناوين
        public DbSet<Address> Addresses { get; set; }

        // ============================================
        // OnModelCreating - إعدادات الموديلات
        // ============================================

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // مهم جداً: استدعاء الـ base لإعداد جداول Identity
            base.OnModelCreating(builder);

            // تطبيق جميع الـ Configurations من هذا الـ Assembly
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // ============================================
            // إعدادات العلاقات
            // ============================================

            // العلاقة بين Product و Category
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // منع الحذف التلقائي

            // العلاقة بين Order و AppUser
            builder.Entity<Order>()
                .HasOne(o => o.AppUser)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // العلاقة بين OrderItem و Order
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // حذف العناصر عند حذف الطلب

            // العلاقة بين OrderItem و Product
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // العلاقة بين Address و AppUser
            builder.Entity<Address>()
                .HasOne(a => a.AppUser)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ============================================
            // إعدادات الـ Indexes
            // ============================================

            // Index على اسم المنتج للبحث السريع
            builder.Entity<Product>()
                .HasIndex(p => p.Name);

            // Index على اسم الفئة
            builder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique(); // اسم الفئة يجب أن يكون فريداً
        }
    }
}
