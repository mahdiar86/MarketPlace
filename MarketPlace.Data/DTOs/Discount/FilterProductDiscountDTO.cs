using MarketPlace.Data.DTOs.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketPlace.Data.Entities.Products;

namespace MarketPlace.Data.DTOs.Discount
{
    public class FilterProductDiscountDTO : BasePaging
    {
        public long? ProductId { get; set; }
        public long? SellerId { get; set; }

        public List<ProductDiscount> ProductDiscounts { get; set; }

        public FilterProductDiscountDTO SetDiscounts(List<ProductDiscount> productDiscounts)
        {
            this.ProductDiscounts = productDiscounts;
            return this;
        }

        public FilterProductDiscountDTO SetPaging(BasePaging paging)
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
