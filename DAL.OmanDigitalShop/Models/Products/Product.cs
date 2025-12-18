using DAL.OmanDigitalShop.Models.Products.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.OmanDigitalShop.Models.Products
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int NoInStock { get; set; }
        public int NoOfOrders { get; set; }
        public int  NumberOfRefund { get; set; }

        public decimal Price { get; set; }
        public bool HasDiscount { get; set; }
        public DiscountCat discountType { get; set; }

        public DateTime CreatedAt { get; set; }


        //Navigational and relational
        [ForeignKey(nameof(category))]
        public int CategoryId { get; set; }
        public Category? category { get; set; }
    }
}
