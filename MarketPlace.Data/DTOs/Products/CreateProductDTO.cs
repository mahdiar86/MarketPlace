using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MarketPlace.Data.Entities.Products;

namespace MarketPlace.Data.DTOs.Products
{
    public class CreateProductDTO
    {
        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }

        [Display(Name = "درصد تخفیف محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Range(0, 100, ErrorMessage = "درصد تخفیف میبایست بین 0 الی 100 درصد باشد")]
        public int DiscountPercent { get; set; }

        [Display(Name = "توضیحات کوتاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ShortDescription { get; set; }

        [Display(Name = "توضیحات اصلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "فعال / غیرفعال")]
        public bool IsActive { get; set; }

        [Display(Name = "تعداد موجودی در فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int QuantityInShop { get; set; }

        [Display(Name = "درصد پورسانت سایت")]
        [Range(0, 100, ErrorMessage = "{0} باید بین 0 و 100 باشد")]
        public int SiteProfit { get; set; }

        //public List<CreateProductColorDTO> ProductColors { get; set; }
        public List<CreateProductFeatureDTO> ProductFeatures { get; set; }
        public List<CreateProductTechnicalFeatureDTO> TechnicalFeatures { get; set; }
        public List<long> SelectedCategories { get; set; }
    }

    public enum CreateProductResult
    {
        Success, Error, HasNoImage, HasNoDescription
    }
}
