using System;
using System.Threading.Tasks;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Application.Utils;
using MarketPlace.Data.DTOs.Seller;
using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Areas.User.Controllers
{
    public class SellerController : UserBaseController
    {
        #region Constrcutor

        private readonly ISellerService _sellerService;

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        #endregion

        #region Request Seller

        [HttpGet("RequestSellerPanel")]
        public IActionResult RequestSellerPanel()
        {
            return View();
        }

        [HttpPost("RequestSellerPanel"), ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestSellerPanel(RequestSellerDTO sellerRequest, IFormFile certificate)
        {
            if (string.IsNullOrEmpty(sellerRequest.AdminDescription))
            {
                TempData[ErrorMessage] = "لطفا توضیحات بخش ادمین را وارد کنید";
                return View(sellerRequest);
            }

            if (certificate == null)
            {
                TempData[ErrorMessage] = "لطفا تصویر شناسنامه خود را وارد کنید";
                return View(sellerRequest);
            }

            if (!certificate.IsImage())
            {
                TempData[WarningMessage] = "تصویر وارد شده معتبر نمی باشد";
                return View(sellerRequest);
            }

            if (ModelState.IsValid)
            {
                var result = await _sellerService.AddNewSellerRequest(sellerRequest, certificate, User.GetUserId());

                switch (result)
                {
                    case RequestSellerResult.HasUnderProgressRequest:
                        TempData[InfoMessage] = "درخواست های قبلی شما در حال پیگیری می باشند";
                        break;
                    case RequestSellerResult.HasNotPermission:
                        TempData[ErrorMessage] = "شما دسترسی لازم را جهت ثبت فروشگاه ندارید !";
                        break;
                    case RequestSellerResult.Error:
                        TempData[ErrorMessage] = "خطایی رخ داده است";
                        break;
                    case RequestSellerResult.Success:
                        TempData[SuccessMessage] = "درخواست شما با موفقیت ثبت شد , تا 72 ساعت دیگر نتیجه درخواست شما ارسال خواهد شد ";
                        return RedirectToAction("SellerRequests");
                }

            }
            return View(sellerRequest);
        }

        #endregion

        #region Seller Requests

        [HttpGet("SellerRequests")]
        public async Task<IActionResult> SellerRequests(FilterSellerDTO filter)
        {
            filter.TakeEntity = 9;
            filter.UserId = User.GetUserId();

            return View(await _sellerService.FilterSellers(filter));
        }

        #endregion

        #region Edit Seller Request

        [HttpGet("EditSellerRequest/{id}")]
        public async Task<IActionResult> EditSellerRequest(long id)
        {
            var request = await _sellerService.GetRequestSellerForEdit(id, User.GetUserId());

            if (request == null)
                return NotFound();

            return View(request);
        }

        [HttpPost("EditSellerRequest/{id}")]
        public async Task<IActionResult> EditSellerRequest(EditRequestSellerDTO request)
        {
            if (string.IsNullOrEmpty(request.AdminDescription))
            {
                ViewData[ErrorMessage] = "لطفا توضیحات بخش ادمین را وارد کنید !";
                return View(request);
            }

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _sellerService.EditRequestSeller(request, User.GetUserId());

            switch (result)
            {
                case EditRequestSellerResult.Success:
                    TempData[SuccessMessage] = "درخواست فروشندگی شما با موفقیت ویرایش شد !";
                    return RedirectToAction("SellerRequests", "Seller");
                case EditRequestSellerResult.Error:
                    return NotFound();
            }

            return View();
        }

        #endregion
    }
}
