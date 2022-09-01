using System.Threading.Tasks;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Data.DTOs.Seller;
using MarketPlace.Data.Entities.Store;
using MarketPlace.Web.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Areas.Admin.Controllers
{
    public class SellerController : AdminBaseController
    {
        #region constrcutor

        private readonly ISellerService _sellerService;

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        #endregion

        #region Seller Requests

        public async Task<IActionResult> SellerRequests(FilterSellerDTO filter)
        {
            return View(await _sellerService.FilterSellers(filter));
        }

        #endregion

        #region Accept Seller Request

        public async Task<IActionResult> AcceptSellerRequest(long requestId)
        {
            var result = await _sellerService.GetSellerRequest(requestId);

            if (result == null)
                return NotFound();

            //if (result)
            //{
            //    return JsonResponseResult.SendStatus(
            //        JsonResultStatusType.Success,
            //        "درخواست مورد نظر با موفقیت تایید شد !",
            //        null);
            //}

            //return JsonResponseResult.SendStatus(
            //    JsonResultStatusType.Error,
            //    "اطلاعاتی با مشخصات وارد شده یافت نشد !",
            //    null);

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptSellerRequest(Data.Entities.Store.Seller request)
        {
            var result = await _sellerService.AcceptSellerRequest(request.Id);

            return RedirectToAction("SellerRequests");
        }

        #endregion

        #region Reject Seller Request

        public async Task<IActionResult> RejectSellerRequest(long requestId)
        {
            var result = await _sellerService.GetSellerRequest(requestId);

            if (result == null)
                return NotFound();

            if (result.StoreAcceptanceState != StoreAcceptanceState.Rejected)
            {
                bool status = await _sellerService.RejectSellerRequest(result);
            }

            return RedirectToAction("SellerRequests");
        }

        #endregion
    }
}
