using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Data.DTOs.Products;

namespace MarketPlace.Web.Controllers
{
    public class ProductController : SiteBaseController
    {
        #region constrcutor

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region filter products

        [HttpGet("Products")]
        public async Task<IActionResult> FilterProducts(FilterProductDTO filter)
        {
            filter.TakeEntity = 16;
            var products = await _productService.FilterProducts(filter);
            ViewBag.ProductCategories = await _productService.GetAllActiveProductCategories();

            //if (filter.PageId > filter.GetLastPage())
            //    return NotFound();

            return View(products);
        }

        #endregion

        #region show product detail

        [HttpGet("products/{productId}/{title}")]
        public async Task<IActionResult> ProductDetail(long productId, string title)
        {
            var product = await _productService.GetProductDetailById(productId);
            if (product == null)
                return NotFound();

            return View(product);
        }

        #endregion
    }
}
