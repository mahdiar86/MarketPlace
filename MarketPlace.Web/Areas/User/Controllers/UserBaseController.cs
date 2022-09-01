using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    [Route("User")]
    public class UserBaseController : Controller
    {
        protected string ErrorMessage = "ErrorMessage";
        protected string InfoMessage = "InfoMessage";
        protected string WarningMessage = "WarningMessage";
        protected string SuccessMessage = "SuccessMessage";
    }
}
