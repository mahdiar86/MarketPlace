using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Data.DTOs.Products;
using MarketPlace.Web.Http;

namespace MarketPlace.Web.Areas.Admin.Controllers
{
    public class ProductsController : AdminBaseController
    {
        #region constrcutor

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        public async Task<IActionResult> Index(FilterProductDTO filter)
        {
            return View(await _productService.FilterProducts(filter));
        }
        
        public async Task<IActionResult> AcceptSellerProduct(long id)
        {
            var result = await _productService.AcceptSellerProduct(id);
            if(result)
            {
                return JsonResponseResult.SendStatus(JsonResultStatusType.Success, "محصول مورد نظر با موفقیت تایید شد !", null);
            }

            return JsonResponseResult.SendStatus(JsonResultStatusType.Error,"محصول مورد نظر یافت نشد",null);
        }

        public async Task<IActionResult> RejectSellerProduct(RejectProductDTO reject)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.RejectSellerProduct(reject);

                if (result)
                {
                    return JsonResponseResult.SendStatus(JsonResultStatusType.Success,"محصول مورد نظر با موفقیت رد شد",null);
                }
            }

            return JsonResponseResult.SendStatus(JsonResultStatusType.Success, "محصول مورد نظر یافت نشد ", null);
        }
    }
}
