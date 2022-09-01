﻿using MarketPlace.Data.DTOs.Paging;
using MarketPlace.Data.DTOs.Seller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Data.DTOs.SellerWallet
{
    public class FilterSellerWalletDTO : BasePaging
    {
        public long? SellerId { get; set; }

        public int? PriceFrom { get; set; }

        public int? PriceTo { get; set; }

        public List<MarketPlace.Data.Entities.Wallet.SellerWallet> SellerWallets { get; set; }

        public FilterSellerWalletDTO SetSellerWallet(List<MarketPlace.Data.Entities.Wallet.SellerWallet> wallets)
        {
            SellerWallets = wallets;
            return this;
        }

        public FilterSellerWalletDTO SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            this.PageCount = paging.PageCount;

            return this;
        }
    }
}
