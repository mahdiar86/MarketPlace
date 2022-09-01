using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Areas.Seller.Controllers
{
    public class HomeController : SellerBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
