using System.Threading.Tasks;
using MarketPlace.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Areas.Seller.ViewComponents
{
    public class SellerSideBar : ViewComponent
    {
        private readonly IUserService _userService;

        public SellerSideBar(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.user = _userService.GetUserByMobile(User.Identity.Name);
            
            return View("SellerSideBar");
        }
    }
}
