using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketPlace.Data.Entities.Products;

namespace MarketPlace.Data.DTOs.Products
{
    public class ProductDetailDTO
    {
        public long ProductId { get; set; }

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

        public List<Product> RelatedProducts { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; }

        //public ICollection<ProductColor> ProductColors { get; set; }

        public ICollection<ProductFeature> ProductFeatures { get; set; }

        public ICollection<TechnicalFeatures> TechnicalFeatures { get; set; }

        public Entities.Store.Seller Seller { get; set; }

        public ICollection<ProductGallery> ProductGalleries { get; set; }
    }
}
