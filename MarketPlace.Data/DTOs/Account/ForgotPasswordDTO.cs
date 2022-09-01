using MarketPlace.Data.DTOs.Site;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Data.DTOs.Account
{
    public class ForgotPasswordDTO : CapchaViewModel
    {
        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا تلفن همراه را وارد کنید")]
        [MaxLength(40, ErrorMessage = " نمی تواند بیش از {1} کاراکتر باشد {0}")]
        public string Mobile { get; set; }
    }

    public enum ForgotPasswordResult
    {
        Success, NotFound , Error
    }
}
