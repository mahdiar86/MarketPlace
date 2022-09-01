using MarketPlace.Application.Services.Implementation;
using MarketPlace.Data.DTOs.Seller;
using MarketPlace.Data.DTOs.SellerWallet;
using MarketPlace.Data.Entities.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Interfaces
{
    public interface ISellerWalletService
    {
        Task<FilterSellerWalletDTO> FilterSellerWalletDTO(FilterSellerWalletDTO filter);

        Task AddWallet(SellerWallet wallet);
    }
}
