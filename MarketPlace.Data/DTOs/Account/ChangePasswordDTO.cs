using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Data.DTOs.Account
{
    public class ChangePasswordDTO
    {
        [Display(Name = "کلمه عبور فعلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = " نمی تواند بیش از {1} کاراکتر باشد {0}")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "کلمه عبور نمی تواند کمتر از 4 کاراکتر باشد")]
        public string CurrentPassword { get; set; }

        [Display(Name = "کلمه عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = " نمی تواند بیش از 100 کاراکتر باشد {0}")]
        [MinLength(4, ErrorMessage = "کلمه عبور نمی تواند کمتر از 4 کاراکتر باشد")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "تکرار کلمه عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = " نمی تواند بیش از 100 کاراکتر باشد {0}")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "کلمه های عبور جدید مغایرت دارند")]
        [MinLength(4, ErrorMessage = "کلمه عبور نمی تواند کمتر از 4 کاراکتر باشد")]
        public string ConfirmNewPassword { get; set; }
    }

    public enum ChangePasswordResult
    {
        Success, PasswordInValid , Error
    }

}
