using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Application.Utils;
using MarketPlace.Data.DTOs.Discount;
using MarketPlace.Data.DTOs.Paging;
using MarketPlace.Data.Entities.Products;
using MarketPlace.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Implementation
{
    public class DiscountService : IDiscountService
    {
        #region Constructor

        private readonly IGenericRepository<ProductDiscount> _productDiscountRepository;
        private readonly IGenericRepository<ProductDiscountUsage> _productDiscountUsageRepository;
        private readonly IGenericRepository<Product> _productRepository;

        public DiscountService(IGenericRepository<ProductDiscount> productDiscountRepository, IGenericRepository<ProductDiscountUsage> productDiscountUsageRepository, IGenericRepository<Product> productRepository)
        {
            _productDiscountRepository = productDiscountRepository;
            _productDiscountUsageRepository = productDiscountUsageRepository;
            _productRepository = productRepository;
        }

        #endregion

        #region Filter Product Discount

        public async Task<FilterProductDiscountDTO> FilterProductDiscount(FilterProductDiscountDTO filter)
        {
            var query = _productDiscountRepository
                .GetQuery()
                .Include(e=>e.Product)
                .AsQueryable();

            if (filter.ProductId != null && filter.ProductId != 0)
                query = query.Where(p=>p.ProductId == filter.ProductId.Value);

            if (filter.SellerId != null && filter.SellerId != 0)
                query = query.Where(p => p.Product.SellerId == filter.SellerId.Value);

            var pager = Pager.Build(filter.PageId, await query.CountAsync(),
                filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntities = query.OrderByDescending(d => d.CreateDate).ToList().PagingList(pager).ToList();

            return filter.SetPaging(pager).SetDiscounts(allEntities);
        }

        #endregion

        #region Create Product Discount 

        public async Task<CreateDiscountResult> CreateProductDiscount(CreateProductDiscountDTO discount, long sellerId)
        {
            var product = await _productRepository.GetEntityById(discount.ProductId);

            if(product == null)
                return CreateDiscountResult.ProductNotFound;
            if (product.SellerId != sellerId)
                return CreateDiscountResult.ProductIsNotForSeller;

            var newDiscount = new ProductDiscount
            {
                ProductId = product.Id,
                DiscountNumber = product.DiscountPercent,
                Percentage = product.DiscountPercent,
                ExpireDate = discount.ExpireDate.ToMiladiDateTime()
            };

            await _productDiscountRepository.AddEntity(newDiscount);
            await _productDiscountRepository.SaveChanges();

            return CreateDiscountResult.Success;
        }


        #endregion

        #region Dispose

        public async ValueTask DisposeAsync()
        {
            await _productDiscountRepository.DisposeAsync();
            await _productDiscountUsageRepository.DisposeAsync();
        }

        #endregion
    }
}
