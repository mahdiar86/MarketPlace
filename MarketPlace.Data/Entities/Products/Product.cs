using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using MarketPlace.Data.Entities.Common;
using MarketPlace.Data.Entities.ProductOrder;
using MarketPlace.Data.Entities.Store;

namespace MarketPlace.Data.Entities.Products
{
    public class Product : BaseEntity
    {
        #region properties

        public long SellerId { get; set; }

        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "تصویر محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }

        [Display(Name = "درصد تخفیف محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [MinLength(0, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد")]
        public int DiscountPercent { get; set; }

        [Display(Name = "توضیحات کوتاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ShortDescription { get; set; }

        [Display(Name = "توضیحات اصلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "پیام تایید/عدم تایید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ProductAcceptOrRejectDescription { get; set; }

        [Display(Name = "فعال / غیرفعال")]
        public bool IsActive { get; set; }

        [Display(Name = "بازدید")]
        public long Visit { get; set; }

        [Display(Name = "تعداد موجودی در فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MinLength(1, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد")]
        public int QuantityInShop { get; set; }

        [Display(Name = "درصد پورسانت سایت")]
        [Range(0,100, ErrorMessage = "{0} باید بین 0 و 100 باشد")]
        public int SiteProfit { get; set; }

        [Display(Name = "وضعیت")]
        public ProductAcceptanceState ProductAcceptanceState { get; set; }

        #endregion

        #region relations

        public ICollection<ProductSelectedCategory> ProductSelectedCategories { get; set; }

        //public ICollection<ProductColor> ProductColors { get; set; }

        public ICollection<ProductFeature> ProductFeatures { get; set; }

        public Seller Seller { get; set; }

        public ICollection<ProductGallery> ProductGalleries { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public ICollection<TechnicalFeatures> TechnicalFeatures { get; set; }

        public ICollection<ProductDiscount> ProductDiscounts { get; set; }

        #endregion
    }

    public enum ProductAcceptanceState
    {
        [Display(Name = "درحال بررسی")]
        UnderProgress,
        [Display(Name = "تایید شده")]
        Accepted,
        [Display(Name = "رد شده")]
        Rejected
    }
}
