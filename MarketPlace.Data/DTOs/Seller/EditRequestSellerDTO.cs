using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Data.DTOs.Seller
{
    public class EditRequestSellerDTO
    {
        public long Id { get; set; }

        [Display(Name = "نام فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string StoreName { get; set; }

        [Display(Name = "تلفن فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [MinLength(6, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string StorePhone { get; set; }

        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [MinLength(6, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Mobile { get; set; }

        [Display(Name = "آدرس فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Address { get; set; }

        [Display(Name = "توضیحات (بخش مدیریت)")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string AdminDescription { get; set; }
    }

    public enum EditRequestSellerResult
    {
        Success,Error
    }
}
