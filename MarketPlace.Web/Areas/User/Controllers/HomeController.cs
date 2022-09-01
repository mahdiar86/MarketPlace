using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Areas.User.Controllers
{
    public class HomeController : UserBaseController
    {
        #region Constrcutor



        #endregion

        #region User Dashboard

        [HttpGet("")]
        public IActionResult Dashboard()
        {
            return View();
        }

        #endregion

    }
}
