using MarketPlace.Web.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Areas.Seller.Controllers
{
    [Authorize]
    [Area("Seller")]
    [Route("Seller")]
    [CheckSellerState]
    public class SellerBaseController : Controller
    {
        protected string ErrorMessage = "ErrorMessage";
        protected string InfoMessage = "InfoMessage";
        protected string WarningMessage = "WarningMessage";
        protected string SuccessMessage = "SuccessMessage";
    }
}
