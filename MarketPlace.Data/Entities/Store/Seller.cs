using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MarketPlace.Data.Entities.Account;
using MarketPlace.Data.Entities.Common;
using MarketPlace.Data.Entities.Wallet;

namespace MarketPlace.Data.Entities.Store
{
    public class Seller : BaseEntity
    {
        public long UserId { get; set; }

        [Display(Name = "نام فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string StoreName { get; set; }

        [Display(Name = "نام اختصاری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string StoreId { get; set; }

        [Display(Name = "تصویر اسکن شناسنامه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string BirthCertificateScan { get; set; }

        [Display(Name = "تلفن فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [MinLength(6, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد")]
        public string StorePhone { get; set; }

        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [MinLength(6, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد")]
        public string Mobile { get; set; }

        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [MinLength(6, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد")]
        public string NationalCode { get; set; }

        [Display(Name = "آدرس فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Address { get; set; }

        [Display(Name = "تصویر لوگو")]
        [MaxLength(150, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Logo { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "توضیحات (بخش مدیریت)")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string AdminDescription { get; set; }

        [Display(Name = "توضیحات تایید / عدم تایید اطلاعات")]
        public string StoreAcceptanceDescription { get; set; }


        public StoreAcceptanceState StoreAcceptanceState { get; set; } 
        public User User { get; set; }
        public ICollection<SellerWallet> SellerWallets { get; set; }
    }

    public enum StoreAcceptanceState
    {
        [Display(Name = "درحال بررسی")] UnderProgress,
        [Display(Name = "تایید شده")] Accepted,
        [Display(Name = "رد شده")] Rejected
    }
}
