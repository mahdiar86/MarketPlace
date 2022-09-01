using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Data.DTOs.SellerWallet;
using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarketPlace.Web.Areas.Seller.Controllers
{
    public class SellerWalletController : SellerBaseController
    {
        #region Constructor

        private readonly ISellerWalletService _sellerWalletService;
        private readonly ISellerService _sellerService;

        public SellerWalletController(ISellerWalletService sellerWalletService, ISellerService sellerService)
        {
            _sellerWalletService = sellerWalletService;
            _sellerService = sellerService;
        }

        #endregion

        #region Seller Wallet

        [HttpGet("seller-wallet")]
        public async Task<IActionResult> Index(FilterSellerWalletDTO filter)
        {
            filter.TakeEntity = 12;
            var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());

            if (seller == null)
                return NotFound();

            filter.SellerId = seller.Id;

            return View(await _sellerWalletService.FilterSellerWalletDTO(filter));
        }

        #endregion


    }
}
