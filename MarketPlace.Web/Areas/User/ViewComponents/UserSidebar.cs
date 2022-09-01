using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Areas.User.ViewComponents
{
    public class UserSidebar : ViewComponent
    {
        private readonly IUserService _userService;
        private ISellerService _sellerService;

        public UserSidebar(IUserService userService, ISellerService sellerService)
        {
            _userService = userService;
            _sellerService = sellerService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.user = await _userService.GetUserByMobile(User.Identity.Name);
            ViewBag.hasUserAnyActiveSellerPanel = await _sellerService.HasUserActiveSeller(User.GetUserId());

            return View("/Areas/User/Views/Shared/Components/UserSidebar/Usersidebar.cshtml");
        }
    }
}
