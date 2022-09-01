using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Data.DTOs.Discount;
using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarketPlace.Web.Areas.Seller.Controllers
{
    //public class ProductDiscountController : SellerBaseController
    //{
    //    #region Constructor

    //    private readonly IDiscountService _discountService;
    //    private readonly ISellerService _sellerService;

    //    public ProductDiscountController(IDiscountService discountService, ISellerService sellerService)
    //    {
    //        _discountService = discountService;
    //        _sellerService = sellerService;
    //    }

    //    #endregion

    //    #region Filter Discount

    //    [HttpGet("discounts")]
    //    [HttpGet("discounts/{ProductId}")]
    //    public async Task<IActionResult> FilterDiscounts(FilterProductDiscountDTO filter)
    //    {
    //        //if(filter.ProductId == null || filter.ProductId == 0)
    //        //    return NotFound();

    //        var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
    //        filter.SellerId = seller.Id;

    //        return View(await _discountService.FilterProductDiscount(filter));
    //    }

    //    #endregion

    //    #region Create Discount

    //    [HttpGet("create-discount")]
    //    public async Task<IActionResult> CreateDiscount()
    //    {
    //        return View();
    //    }

    //    [HttpPost("create-discount"), ValidateAntiForgeryToken]
    //    public async Task<IActionResult> CreateDiscount(CreateProductDiscountDTO discount)
    //    {
    //        if(ModelState.IsValid)
    //        {
    //            var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
    //            var res = await _discountService.CreateProductDiscount(discount, seller.Id);

    //            switch (res)
    //            {
    //                case CreateDiscountResult.Error:
    //                    TempData[ErrorMessage] = "عملیات ثبت تخفیف با شکست مواجه شد";
    //                    break;
    //                case CreateDiscountResult.ProductNotFound:
    //                    TempData[WarningMessage] = "محصول مورد نظر یافت نشد";
    //                    break;
    //                case CreateDiscountResult.ProductIsNotForSeller:
    //                    TempData[ErrorMessage] = "شما دسترسی افزودن تخفیف برای این محصول را ندارید !";
    //                    break;
    //                case CreateDiscountResult.Success:
    //                    TempData[SuccessMessage] = "عملیات ثبت تخفیف برای محصول مورد نظر با موفقیت انجام شد !";
    //                    return RedirectToAction("FilterDiscounts");
    //            }

    //        }

    //        return View();
    //    }

    //    #endregion
    //}
}
