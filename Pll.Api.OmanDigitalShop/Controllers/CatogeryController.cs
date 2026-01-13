using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SLL.OmanDigitalShop.Interfaces;

namespace Pll.Api.OmanDigitalShop.Controllers
{
   
    public class CatogeryController : BaseController
    {
        private readonly ICategoryService CatogeryService;

        public CatogeryController(ICategoryService CatogeryService)
        {
            this.CatogeryService = CatogeryService;
        }



    }
}
