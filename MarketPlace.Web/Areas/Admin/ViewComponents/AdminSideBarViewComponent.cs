using System.Threading.Tasks;
using MarketPlace.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Areas.Admin.ViewComponents
{
    public class AdminSideBarViewComponent : ViewComponent
    {
        private readonly IUserService _userService;

        public AdminSideBarViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.user = await _userService.GetUserByMobile(User.Identity.Name);


            return View();
        }
    }
}
