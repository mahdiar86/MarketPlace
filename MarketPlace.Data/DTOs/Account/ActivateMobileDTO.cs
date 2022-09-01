using MarketPlace.Data.DTOs.Site;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Data.DTOs.Account
{
    public class ActivateMobileDTO : CapchaViewModel
    {
        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا تلفن همراه را وارد کنید")]
        [MaxLength(40, ErrorMessage = " نمی تواند بیش از {1} کاراکتر باشد {0}")]
        public string Mobile { get; set; }

        [Display(Name = "کد فعال سازی تلفن همراه")]
        [Required(ErrorMessage = "لطفا کد فعال سازی تلفن همراه را وارد کنید")]
        [MaxLength(40, ErrorMessage = " نمی تواند بیش از {1} کاراکتر باشد {0}")]
        public string MobileActiveCode { get; set; }
    }

    public enum ActivateMobileResult
    {

    }
}
