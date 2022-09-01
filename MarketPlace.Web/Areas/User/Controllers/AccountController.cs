using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Data.DTOs.Account;
using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Http;

namespace MarketPlace.Web.Areas.User.Controllers
{
    public class AccountController : UserBaseController
    {
        #region Constrcutor

        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }


        #endregion

        #region ChangePassword

        [HttpGet("ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost("ChangePassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO password)
        {
            if (ModelState.IsValid)
            {
                var result =  await _userService.ChangeUserPassword(password, User.Identity.Name.ToString());
                if (result == ChangePasswordResult.Success)
                {
                    TempData[SuccessMessage] = "کلمه عبور شما با موفقیت ویرایش شد !";
                    return View();
                }
                if (result == ChangePasswordResult.PasswordInValid)
                {
                    TempData[ErrorMessage] = "کلمه عبور فعلی شما نادرست می باشد";
                    return View();
                }
                else if (result == ChangePasswordResult.Error)
                {
                    TempData[ErrorMessage] = "خطایی رخ داده است";
                    return View();
                }
            }

            return View();
        }

        #endregion

        #region Edit Profile

        [HttpGet("EditProfile")]
        public async Task<IActionResult> EditProfile()
        {
            var userProfile = await _userService.GetProfileForEdit(User.GetUserId());
            if (userProfile == null)
                return NotFound();

            return View(userProfile);
        }

        [HttpPost("EditProfile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditUserProfileDTO profile,IFormFile avatarImage)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.EditUserProfile(profile,User.GetUserId(),avatarImage);
                switch (result)
                {
                    case EditUserProfileResult.IsBlocked:
                        TempData[ErrorMessage] = "حساب کاربری شما بلاک می باشد !";
                        break;
                    case EditUserProfileResult.IsNotActive:
                        TempData[WarningMessage] = "حساب کاربری شما فعال نشده است !";
                        break;
                    case EditUserProfileResult.NotFound:
                        TempData[ErrorMessage] = "کاربری با مشخصات وارد شده یافت نشد !";
                        break;
                    case EditUserProfileResult.Success:
                        TempData[SuccessMessage] = "اطلاعات حساب کاربری شما با موفقیت ویرایش شد !";
                        return RedirectToAction("EditProfile");
                        break;
                }
            }

            return View(profile);
        }

        #endregion

    }
}
