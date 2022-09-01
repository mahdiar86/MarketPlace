using MarketPlace.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MarketPlace.Web.PresentationExtensions;

namespace MarketPlace.Web.ViewComponents
{
    public class SiteHeaderViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        public SiteHeaderViewComponent(ISiteService siteService, IUserService userService, IProductService productService)
        {
            _siteService = siteService;
            _userService = userService;
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.SiteSetting = await _siteService.GetDefaultSiteSettings();

            ViewBag.user = null;
            if (User.Identity.IsAuthenticated)
                ViewBag.user = await _userService.GetUserByMobile(User.Identity.Name);

            ViewBag.categories = await _productService.GetAllActiveProductCategories();
            return View("SiteHeader");
        }
    }

    public class SiteFooterViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;
        public SiteFooterViewComponent(ISiteService siteService)
        {
            _siteService = siteService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.SiteSetting = await _siteService.GetDefaultSiteSettings();
            
            return View("SiteFooter");
        }
    }

    public class HomeSliderViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;
        public HomeSliderViewComponent(ISiteService siteService)
        {
            _siteService = siteService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sliders = await _siteService.GetAllActiveSliders();
            return View("HomeSlider", sliders);
        }
    }

    public class UserOrderViewComponent : ViewComponent
    {
        private readonly IOrderService _orderService;
        public UserOrderViewComponent(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var openOrder = await _orderService.GetUserLatestOpenOrder(User.GetUserId());
            
            return View("UserOrder", openOrder);
        }
    }
}
