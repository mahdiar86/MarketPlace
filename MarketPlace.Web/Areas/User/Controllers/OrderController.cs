using System.Threading.Tasks;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Application.Utils;
using MarketPlace.Data.DTOs.Orders;
using MarketPlace.Web.Http;
using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MarketPlace.Web.Areas.User.Controllers
{
    public class OrderController : UserBaseController
    {
        #region Constructor

        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IPaymentService _paymentService;
        public OrderController(IOrderService orderService, IUserService userService, IPaymentService paymentService)
        {
            _orderService = orderService;
            _userService = userService;
            _paymentService = paymentService;
        }

        #endregion

        #region Add Product To Open Order

        [HttpPost("add-product-to-order")]
        public async Task<IActionResult> AddProductToOrder(AddProductToOrderDTO order)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    await _orderService.AddProductToOpenOrder(order, User.GetUserId());
                    return JsonResponseResult.SendStatus(JsonResultStatusType.Success, "محصول مورد نظر با موفقیت ثبت شد !", null);
                }
                else
                {
                    return JsonResponseResult.SendStatus(JsonResultStatusType.Warning, "برای افزودن محصول به سبد خرید لطفا وارد حساب کاربری خود شوید", null);
                }
            }

            return JsonResponseResult.SendStatus(JsonResultStatusType.Error, "در ثبت اطلاعات خطایی رخ داده است !", null);
        }

        #endregion

        #region Open Cart

        [HttpGet("basket")]
        public async Task<IActionResult> UserOpenOrder()
        {
            return View(await _orderService.GetUserOpenOrderDetail(User.GetUserId()));
        }

        #endregion

        #region Open Order Partial

        [HttpGet("change-open-order/{detailId}/{count}")]
        public async Task<IActionResult> OpenOrderPartial(long detailId, int count)
        {
            var result = await _orderService.ChangeOrderDetailCount(detailId, count, User.GetUserId());
            var openOrder = await _orderService.GetUserOpenOrderDetail(User.GetUserId());

            return PartialView(openOrder);
        }

        #endregion

        #region Remove Product From Order

        [HttpGet("remove-order-item/{detailId}")]
        public async Task<IActionResult> RemoveProductFromOrder(long detailId)
        {
            var res = await _orderService.RemoveOrderDetail(detailId, User.GetUserId());
            if (res)
                return JsonResponseResult.SendStatus(JsonResultStatusType.Success, "محصول مورد نظر با موفقیت از سبد خرید حذف شد !", null);

            return JsonResponseResult.SendStatus(JsonResultStatusType.Error, "خطایی رخ داده است", null);
        }

        #endregion

        #region Pay Order

        [HttpGet("pay-order")]
        public async Task<IActionResult> PayUserOrderPrice()
        {
            var openOrderAmount = await _orderService.GetTotalOrderPriceForPayment(User.GetUserId());
            var user = await _userService.GetUserById(User.GetUserId());

            string callBackUrl = FilePath.DomainAddress + Url.RouteUrl("ZarinPalPaymentResult");
            string redirectUrl = "";

            var status = _paymentService.CreatePaymentRequest(null, (int)openOrderAmount, "تکمیل فرایند خرید از مارکت پلیس", callBackUrl, ref redirectUrl, user.Email, user.Mobile);

            if (status == Data.DTOs.Payment.PaymentStatus.St100)
                return Redirect(redirectUrl);


            return RedirectToAction(nameof(PayUserOrderPrice));
        }

        #endregion

        #region Callback ZarinPal

        [HttpGet("payment-result", Name = "ZarinPalPaymentResult"),AllowAnonymous]
        public async Task<IActionResult> CallbackZarinpal()
        {
            string authority = _paymentService.GetAuthorityCodeFromCallback(HttpContext);
            if(authority == "")
            {
                TempData[WarningMessage] = "عملیات پرداخت با شکست مواجه شد";
                return View();
            }

            var openOrderAmount = await _orderService.GetTotalOrderPriceForPayment(User.GetUserId());
            long refId = 0;
            var result = _paymentService.PaymentVerification(null, authority, (int)openOrderAmount, ref refId);

            if(result == Data.DTOs.Payment.PaymentStatus.St100)
            {
                TempData[SuccessMessage] = "پرداخت شما با موفقیت انجام شد";
                TempData[InfoMessage] = "کد پیگیری شما :" + refId;
                await _orderService.CloseUserOpenOrderAfterPayment(User.GetUserId(), refId);

                return View();
            }

            return View();
        }

        #endregion
    }
}
