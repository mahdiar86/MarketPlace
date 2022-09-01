using System;
using System.IO;
using System.Linq;
using MarketPlace.Application.Services.Interfaces;
using System.Threading.Tasks;
using MarketPlace.Application.Extensions;
using MarketPlace.Application.Utils;
using MarketPlace.Data.DTOs.Paging;
using MarketPlace.Data.DTOs.Seller;
using MarketPlace.Data.Entities.Account;
using MarketPlace.Data.Entities.Store;
using MarketPlace.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Application.Services.Implementation
{
    public class SellerService : ISellerService
    {
        #region Constrcutor

        private readonly IGenericRepository<Seller> _sellerRepository;
        private readonly IGenericRepository<User> _userRepository;

        public SellerService(IGenericRepository<Seller> sellerRepository, IGenericRepository<User> userRepository)
        {
            _sellerRepository = sellerRepository;
            _userRepository = userRepository;
        }

        #endregion

        public async Task<RequestSellerResult> AddNewSellerRequest(RequestSellerDTO seller, IFormFile certificate, long userId)
        {
            var user = await _userRepository.GetEntityById(userId);

            if (user.IsBlocked)
                return RequestSellerResult.HasNotPermission;
            if (user.IsDelete)
                return RequestSellerResult.Error;

            var hasUnderProgressRequest = await _sellerRepository.GetQuery().AsQueryable()
                .AnyAsync(u => u.UserId == userId && u.StoreAcceptanceState == StoreAcceptanceState.UnderProgress);

            if (hasUnderProgressRequest)
                return RequestSellerResult.HasUnderProgressRequest;

            if (certificate == null && !certificate.IsImage())
                return RequestSellerResult.Error;

            var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(certificate.FileName);
            certificate.AddImageToServer(imageName, FilePath.UploadCertificateServer, null, null);

            var newSeller = new Seller
            {
                UserId = userId,
                StoreName = seller.StoreName,
                Address = seller.Address,
                StorePhone = seller.StorePhone,
                BirthCertificateScan = imageName,
                Description = seller.Description,
                AdminDescription = seller.AdminDescription,
                StoreId = new Guid().ToString("N").Substring(0, 5) + userId,
                Mobile = seller.Mobile,
                NationalCode = seller.NationalCode
            };

            await _sellerRepository.AddEntity(newSeller);
            await _sellerRepository.SaveChanges();

            return RequestSellerResult.Success;
        }


        public async Task<FilterSellerDTO> FilterSellers(FilterSellerDTO filter)
        {
            var query = _sellerRepository.GetQuery().Include(u => u.User).AsQueryable();

            switch (filter.State)
            {
                case FilterSellerState.All:
                    query = query.Where(t => !t.IsDelete);
                    break;
                case FilterSellerState.UnderProgress:
                    query = query.Where(r => r.StoreAcceptanceState == StoreAcceptanceState.UnderProgress && !r.IsDelete);
                    break;
                case FilterSellerState.Accepted:
                    query = query.Where(q => q.StoreAcceptanceState == StoreAcceptanceState.Accepted && !q.IsDelete);
                    break;
                case FilterSellerState.Rejected:
                    query = query.Where(q => q.StoreAcceptanceState == StoreAcceptanceState.Rejected && !q.IsDelete);
                    break;
            }

            if (filter.StoreName != null && !string.IsNullOrEmpty(filter.StoreName))
                query = query.Where(r => r.StoreName.Contains(filter.StoreName));

            if (filter.StorePhone != null && !string.IsNullOrEmpty(filter.StorePhone))
                query = query.Where(r => r.StorePhone.Contains(filter.StorePhone));

            if (filter.Mobile != null && !string.IsNullOrEmpty(filter.Mobile))
                query = query.Where(r => r.Mobile.Contains(filter.Mobile));

            if (filter.Address != null && !string.IsNullOrEmpty(filter.Address))
                query = query.Where(r => r.Address.Contains(filter.Address));

            var pager = Pager.Build(filter.PageId, await query.CountAsync(),
                filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntities = query.OrderByDescending(d=>d.CreateDate).ToList().PagingList(pager).ToList();

            return filter.SetPaging(pager).SetSellers(allEntities);
        }

        public async Task<EditRequestSellerDTO> GetRequestSellerForEdit(long id, long currentUserId)
        {
            var seller = await _sellerRepository.GetEntityById(id);

            if (seller == null || seller.UserId != currentUserId)
                return null;


            return new EditRequestSellerDTO
            {
                Id = seller.Id,
                StorePhone = seller.StorePhone,
                StoreName = seller.StoreName,
                Address = seller.Address,
                Mobile = seller.Mobile,
                AdminDescription = seller.AdminDescription
            };
        }

        public async Task<EditRequestSellerResult> EditRequestSeller(EditRequestSellerDTO request, long userId)
        {
            var seller = await _sellerRepository.GetEntityById(request.Id);
            
            if (seller == null || seller.UserId != userId)
                return EditRequestSellerResult.Error;

            seller.StorePhone = request.StorePhone;
            seller.Address = request.Address;
            seller.AdminDescription = request.AdminDescription;
            seller.Mobile = request.Mobile;
            seller.StoreName = request.StoreName;
            seller.StoreAcceptanceState = StoreAcceptanceState.UnderProgress;

            _sellerRepository.EditEntity(seller);
            await _sellerRepository.SaveChanges();

            return EditRequestSellerResult.Success;
        }

        public async Task<bool> AcceptSellerRequest(long requestId)
        {
            var sellerRequest = await _sellerRepository.GetEntityById(requestId);
            if (sellerRequest != null)
            {
                sellerRequest.StoreAcceptanceState = StoreAcceptanceState.Accepted;
                sellerRequest.LastUpdateDate = DateTime.Now;
                
                _sellerRepository.EditEntity(sellerRequest);
                await _sellerRepository.SaveChanges();

                return true;
            }

            return false;
        }

        public async Task<bool> RejectSellerRequest(Seller seller)
        {
            try
            {
                seller.StoreAcceptanceState = StoreAcceptanceState.Rejected;
                seller.LastUpdateDate = DateTime.Now;

                _sellerRepository.EditEntity(seller);
                await _sellerRepository.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }

            return true; 
        }

        public async Task<Seller> GetSellerRequest(long requestId)
        {
            return await _sellerRepository.GetEntityById(requestId);
        }

        public async Task<Seller> GetLastActiveSellerByUserId(long userId)
        {
            return await _sellerRepository
                .GetQuery()
                .OrderBy(u => u.CreateDate)
                .LastOrDefaultAsync(s => s.UserId == userId && s.StoreAcceptanceState == StoreAcceptanceState.Accepted);
        }

        public async Task<bool> HasUserActiveSeller(long userId)
        {
            return await _sellerRepository
                .GetQuery()
                .OrderByDescending(u => u.CreateDate)
                .AnyAsync(s => s.UserId == userId && s.StoreAcceptanceState == StoreAcceptanceState.Accepted);
        }

        #region Dispose

        public async ValueTask DisposeAsync()
        {
            await _sellerRepository.DisposeAsync();
        }

        #endregion
    }
}