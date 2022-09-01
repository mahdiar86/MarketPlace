using System;
using System.Threading.Tasks;
using MarketPlace.Data.DTOs.Account;
using MarketPlace.Data.DTOs.Seller;
using MarketPlace.Data.Entities.Store;
using Microsoft.AspNetCore.Http;

namespace MarketPlace.Application.Services.Interfaces
{
    public interface ISellerService : IAsyncDisposable
    {
        Task<RequestSellerResult> AddNewSellerRequest(RequestSellerDTO seller, IFormFile certificate,long userId);

        Task<FilterSellerDTO> FilterSellers(FilterSellerDTO seller);

        Task<EditRequestSellerDTO> GetRequestSellerForEdit(long id, long currentUserId);

        Task<EditRequestSellerResult> EditRequestSeller(EditRequestSellerDTO request, long userId);

        Task<bool> AcceptSellerRequest(long requestId);

        Task<bool> RejectSellerRequest(Seller seller);

        Task<Seller> GetSellerRequest(long requestId);

        Task<Seller> GetLastActiveSellerByUserId(long userId);

        Task<bool> HasUserActiveSeller(long userId);
    }
}