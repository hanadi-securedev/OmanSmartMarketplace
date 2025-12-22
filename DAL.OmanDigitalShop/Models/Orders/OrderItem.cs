// ============================================
// Joonguini - Programming in the Kitchen
// ============================================
// OrderItem Model: موديل عنصر الطلب
// يمثل منتج واحد داخل الطلب مع الكمية والسعر
// ============================================

using DAL.OmanDigitalShop.Models.Products;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.OmanDigitalShop.Models.Orders
{
    /// <summary>
    /// موديل عنصر الطلب - يربط بين الطلب والمنتج
    /// </summary>
    public class OrderItem : BaseEntity
    {
        // ============================================
        // خصائص عنصر الطلب
        // ============================================

        // الكمية المطلوبة من المنتج
        public int Quantity { get; set; }

        // السعر وقت الشراء (مهم لأن سعر المنتج قد يتغير لاحقاً)
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        // ============================================
        // العلاقة مع الطلب (Order)
        // ============================================

        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }

        public virtual Order? Order { get; set; }

        // ============================================
        // العلاقة مع المنتج (Product)
        // ============================================

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public virtual Product? Product { get; set; }

        // ============================================
        // خاصية محسوبة للمجموع
        // ============================================

        [NotMapped] // لن يتم حفظها في الداتابيز
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
