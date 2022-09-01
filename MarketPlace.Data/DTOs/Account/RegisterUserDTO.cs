using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Data.DTOs.Account
{
    public class RegisterUserDTO
    {
        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "لطفا موبایل را وارد کنید")]
        [MaxLength(20, ErrorMessage = " نمی تواند بیش از {1} کاراکتر باشد {0}")]
        public string Mobile { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا کلمه عبور را وارد کنید")]
        [MaxLength(100, ErrorMessage = " نمی تواند بیش از {1} کاراکتر باشد {0}")]
        [DataType(DataType.Password)]
        [MinLength(4,ErrorMessage = "کلمه عبور نمی تواند کمتر از 4 کاراکتر باشد")]
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا تکرار کلمه عبور را وارد کنید")]
        [MaxLength(100, ErrorMessage = " نمی تواند بیش از 100 کاراکتر باشد {0}")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "کلمه های عبور مغایرت دارند")]
        public string ConfirmPassword { get; set; }
    }

    public enum RegisterUserResult
    {
        Success,
        MobileExists
    }
}
