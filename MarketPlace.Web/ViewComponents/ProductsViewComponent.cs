using System.Threading.Tasks;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Data.DTOs.Products;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.ViewComponents
{
    public class ProductsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public ProductsViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("LatestProducts", await _productService.GetLatestProducts(12));
        }
    }

    public class ProductsSliderViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public ProductsSliderViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("SliderProducts", await _productService.GetLatestProducts(12));
        }
    }

    public class ProductSearchViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public ProductSearchViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("ProductSearch", new FilterProductDTO() {});
        }
    }

    public class ProductOffersViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public ProductOffersViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("ProductOffers", await _productService.GetAllProductOffers());
        }
    }
}
