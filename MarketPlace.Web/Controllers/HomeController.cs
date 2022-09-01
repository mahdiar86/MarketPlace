using GoogleReCaptcha.V3.Interface;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Data.DTOs.Contacts;
using MarketPlace.Data.Entities.SiteSettings;
using MarketPlace.Web.Models;
using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MarketPlace.Web.Controllers
{
    public class HomeController : SiteBaseController
    {
        private readonly IContactService _contactService;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly ISiteService _siteService;
        private readonly IPaymentService _paymentService;
        public HomeController(IContactService contactService, ICaptchaValidator captchaValidator,ISiteService siteService, IPaymentService paymentService)
        {
            _contactService = contactService;
            _captchaValidator = captchaValidator;
            _siteService = siteService;
            _paymentService = paymentService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.banners = await _siteService.GetSiteBannersByPlacement(new List<BannerPlacement>()
            {
                BannerPlacement.Home_1,
                BannerPlacement.Home_2,
                BannerPlacement.Home_3,
                BannerPlacement.Home_4,
                BannerPlacement.Home_5,
                BannerPlacement.Home_6,
                BannerPlacement.Home_7
            });

            //string redirectUrl = "";
            //var res = _paymentService.CreatePaymentRequest(null, 10000, "توضیحات", "http://localhost:16985/User/", ref redirectUrl, "koohkan96@gmail.com", "09153255895");
            //if (res == Data.DTOs.Payment.PaymentStatus.St100)
            //    return Redirect(redirectUrl);

            return View();
        }

        [HttpGet("ContactUs")]
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost("ContactUs")]
        public async Task<IActionResult> ContactUs(CreateContactUsDTO contact)
        {
            if(!await _captchaValidator.IsCaptchaPassedAsync(contact.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                return View(contact);
            }

            if (ModelState.IsValid)
            {
                await _contactService.CreateContactUs(contact, HttpContext.GetUserIp() , User.GetUserId());
                TempData[SuccessMessage] = "پیام شما با موفقیت ارسال شد";
            }


            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
