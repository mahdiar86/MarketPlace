using MarketPlace.Data.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Data.Entities.SiteSettings
{
    public class Site : BaseEntity
    {
        #region properties 

        [Display(Name = "تلفن همراه")]
        [MaxLength(40 , ErrorMessage = " نمی تواند بیش از {1} کاراکتر باشد {0}")]
        public string Mobile { get; set; }

        [Display(Name = "تلفن")]
        [MaxLength(40, ErrorMessage = " نمی تواند بیش از {1} کاراکتر باشد {0}")]
        public string Phone { get; set; }

        [Display(Name = "ایمیل")]
        [MaxLength(200, ErrorMessage = " نمی تواند بیش از {1} کاراکتر باشد {0}")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; set; }

        [Display(Name = "متن فوتر")]
        public string FooterText { get; set; }

        [Display(Name = "متن کپی رایت")]
        public string CopyRight { get; set; }

        [Display(Name = "پیش فرض هست / نیست")]
        public bool IsDefault { get; set; }
        

        #endregion
    }
}
