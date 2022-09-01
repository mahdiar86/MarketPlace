using MarketPlace.Data.DTOs.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Interfaces
{
    public interface IDiscountService : IAsyncDisposable
    {
        Task<FilterProductDiscountDTO> FilterProductDiscount(FilterProductDiscountDTO filter);
        Task<CreateDiscountResult> CreateProductDiscount(CreateProductDiscountDTO discount, long sellerId);
    }
}
