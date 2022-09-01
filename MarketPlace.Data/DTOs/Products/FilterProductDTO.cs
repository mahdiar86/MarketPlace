using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketPlace.Data.DTOs.Paging;
using MarketPlace.Data.DTOs.Seller;
using MarketPlace.Data.Entities.Products;

namespace MarketPlace.Data.DTOs.Products
{
    public class FilterProductDTO : BasePaging
    {
        #region Constructor
        public FilterProductDTO()
        {
            FilterProductOrderBy = FilterProductOrderBy.CreateDate_DES;
        }

        #endregion

        #region properties

        public string ProductTitle { get; set; }

        public long? SellerId { get; set; }

        public List<Product> Products { get; set; }

        public FilterProductState FilterProductState { get; set; }

        public FilterProductOrderBy FilterProductOrderBy { get; set; }

        public long MinPrice { get; set; }
        public long MaxPrice { get; set; }

        public bool IsExists { get; set; }
        public List<long> SelectedProductCategories { get; set; }

        #endregion

        #region methods

        public FilterProductDTO SetProducts(List<Product> products)
        {
            this.Products = products;
            return this;
        }

        public FilterProductDTO SetPaging(BasePaging paging)
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

        #endregion
    }

    public enum FilterProductState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "درحال بررسی")]
        UnderProgress,
        [Display(Name = "تایید شده")]
        Accepted,
        [Display(Name = "رد شده")]
        Rejected,
        [Display(Name = "فعال")]
        Active,
        [Display(Name = "غیرفعال")]
        NotActive
    }

    public enum FilterProductOrderBy
    {
        [Display(Name = "تاریخ (نزولی)")]
        CreateDate_DES,
        [Display(Name = "تاریخ (صعودی)")]
        CreateDate_ACS,
        [Display(Name = "قیمت (صعودی)")]
        Price_ACS,
        [Display(Name = "قیمت (نزولی)")]
        Price_DES
    }
}
