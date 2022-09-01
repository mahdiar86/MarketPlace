using MarketPlace.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Controllers
{
    public class SiteBaseController : Controller
    {
        protected string ErrorMessage = "ErrorMessage";
        protected string InfoMessage = "InfoMessage";
        protected string WarningMessage = "WarningMessage";
        protected string SuccessMessage = "SuccessMessage";
    }
}
