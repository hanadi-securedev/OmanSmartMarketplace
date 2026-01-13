using DAL.OmanDigitalShop.Models.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SLL.OmanDigitalShop.Interfaces;

namespace Pll.Api.OmanDigitalShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Product>>> GetAllWithCat()
        {
            var products = await productService.GetAllProductsAsync();
            return Ok(products);
        }
    }
}
