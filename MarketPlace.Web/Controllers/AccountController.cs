using GoogleReCaptcha.V3.Interface;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Data.DTOs.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MarketPlace.Web.Controllers
{
    public class AccountController : SiteBaseController
    {
        #region constructor

        private readonly IUserService _userService;
        private readonly ICaptchaValidator _captchaValidator;
        public AccountController(IUserService userService, ICaptchaValidator captchaValidator)
        {
            _userService = userService;
            _captchaValidator = captchaValidator;
        }

        #endregion

        #region Register

        [HttpGet("Register")]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return Redirect("/");

            return View();
        }

        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserDTO user)
        {
            if (ModelState.IsValid)
            {
                var res = await _userService.RegisterUser(user);
                switch (res)
                {
                    case RegisterUserResult.Success:
                        TempData[InfoMessage] = "کد تایید تلفن همراه برای شما ارسال شد";
                        return RedirectToAction("ActivateMobile", "Account", new { mobile = user.Mobile });

                    case RegisterUserResult.MobileExists:
                        TempData[ErrorMessage] = "تلفن همراه وارد شده تکراری می باشد";
                        ModelState.AddModelError("Mobile", "تلفن همراه وارد شده تکراری می باشد");
                        break;
                }
            }
            return View();
        }

        #endregion

        #region Login

        [HttpGet("Login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return Redirect("/");
            return View();
        }

        [HttpPost("Login"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserDTO login)
        {
            if (ModelState.IsValid)
            {
                var res = await _userService.GetUserForLogin(login);
                switch (res)
                {
                    case LoginUserResult.NotFound:
                        TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد";
                        return View(login);
                    case LoginUserResult.NotActivated:
                        TempData[WarningMessage] = "حساب کاربری شما فعال نشده است";
                        return View(login);
                    case LoginUserResult.Success:

                        var user = await _userService.GetUserByMobile(login.Mobile);

                        var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name,user.Mobile),
                            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                            new Claim("id",user.Id.ToString())
                        };
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        var properties = new AuthenticationProperties()
                        {
                            IsPersistent = login.RememberMe
                        };

                        await HttpContext.SignInAsync(principal, properties);

                        TempData[SuccessMessage] = "عملیات ورود با موفقیت انجام شد";
                        break;
                }
                if (!await _captchaValidator.IsCaptchaPassedAsync(login.Captcha))
                {
                    TempData[ErrorMessage] = "لطفا کپچا را کامل کنید";
                    return View(login);
                }
                return Redirect("/");
            }

            return View();
        }

        #endregion

        #region LogOut

        [HttpGet("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            if (User.Identity.IsAuthenticated)
                await HttpContext.SignOutAsync();

            return Redirect("/");
        }

        #endregion

        #region Forgot Password 

        [HttpGet("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("ForgotPassword"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgot)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(forgot.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشده است";
                return View(forgot);
            }

            if (ModelState.IsValid)
            {
                var result = await _userService.RecoverUserPassword(forgot);
                switch (result)
                {
                    case ForgotPasswordResult.NotFound:
                        TempData[WarningMessage] = "کاربر مورد نظر یافت نشد";
                        break;
                    case ForgotPasswordResult.Error:
                        TempData[ErrorMessage] = "خطایی رخ داده است";
                        break;
                    case ForgotPasswordResult.Success:
                        TempData[SuccessMessage] = "کلمه عبور جدید برای شما ارسال شد , لطفا پس از ورود به حساب کاربری کلمه عبور را تغییر دهید";
                        break;
                }
                return RedirectToAction("Login");
            }

            return View();
        }

        #endregion

        #region Activate Mobile

        [HttpGet("ActivateMobile")]
        public IActionResult ActivateMobile(string mobile)
        {
            if (User.Identity.IsAuthenticated)
                return Redirect("/");

            var activateMobileDTO = new ActivateMobileDTO { Mobile = mobile };

            return View(activateMobileDTO);
        }

        [HttpPost("ActivateMobile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateMobile(ActivateMobileDTO activate)
        {
            if (ModelState.IsValid)
            {
                var res = await _userService.ActivateMobile(activate);
                if (res)
                {
                    TempData[SuccessMessage] = "حساب کاربری شما با موفقیت فعال شد !";
                    return RedirectToAction("Login");
                }
                TempData[ErrorMessage] = "کاربری با مشخصات وارد شده یافت نشد";
            }
            return View();
        }

        #endregion
    }
}
