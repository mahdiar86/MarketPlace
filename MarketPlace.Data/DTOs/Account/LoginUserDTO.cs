using MarketPlace.Data.DTOs.Site;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Data.DTOs.Account
{
    public class LoginUserDTO : CapchaViewModel
    {
        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا تلفن همراه را وارد کنید")]
        [MaxLength(40, ErrorMessage = " نمی تواند بیش از {1} کاراکتر باشد {0}")]
        public string Mobile { get; set; }

        [Display(Name = "کلمه عبور")]

        [MaxLength(100, ErrorMessage = " نمی تواند بیش از {1} کاراکتر باشد {0}")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "لطفا تلفن همراه را وارد کنید")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public enum LoginUserResult
    {
        Success,
        NotFound,
        NotActivated,
        Blocked
    }
}
