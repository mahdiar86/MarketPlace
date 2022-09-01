using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Data.DTOs.Paging;
using MarketPlace.Data.DTOs.SellerWallet;
using MarketPlace.Data.Entities.Wallet;
using MarketPlace.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Implementation
{
    public class SellerWalletService : ISellerWalletService
    {
        #region Constructor

        private readonly IGenericRepository<SellerWallet> _sellerWalletRepository;
        public SellerWalletService(IGenericRepository<SellerWallet> sellerWallterRepository)
        {
            _sellerWalletRepository = sellerWallterRepository;
        }

        #endregion

        #region Add Wallet

        public async Task AddWallet(SellerWallet wallet)
        {
            await _sellerWalletRepository.AddEntity(wallet);
            await _sellerWalletRepository.SaveChanges();
        }

        #endregion

        #region Wallet

        public async Task<FilterSellerWalletDTO> FilterSellerWalletDTO(FilterSellerWalletDTO filter)
        {
            var query = _sellerWalletRepository.GetQuery().AsQueryable();

            if(filter.SellerId != null && filter.SellerId != 0)
                query = query.Where(x => x.SellerId == filter.SellerId.Value);

            if (filter.PriceFrom != null)
                query = query.Where(f=>f.Price >= filter.PriceFrom.Value);

            if (filter.PriceFrom != null)
                query = query.Where(f => f.Price <= filter.PriceTo.Value);

            var allEntitiesCount = await query.CountAsync();
            var pager = Pager.Build(filter.PageId, allEntitiesCount, filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);

            var wallets = await query.Paging(pager).ToListAsync();

            return filter.SetSellerWallet(wallets).SetPaging(pager);
        }

        #endregion
    }
}
