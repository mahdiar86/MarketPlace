using MarketPlace.Data.Entities.Common;
using MarketPlace.Data.Entities.Contacts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketPlace.Data.Entities.Store;
using MarketPlace.Data.Entities.Products;

namespace MarketPlace.Data.Entities.Account
{
    public class User : BaseEntity
    {
        #region properties

        [Display(Name = "ایمیل")]
        [MaxLength(200, ErrorMessage = " نمی تواند بیش از {1} کاراکتر باشد {0}")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "کد فعال سازی ایمیل")]
        [MaxLength(100, ErrorMessage = " نمی تواند بیش از {1} کاراکتر باشد {0}")]
        public string EmailActiveCode { get; set; }

        [Display(Name = "ایمیل فعال / غیرفعال")]
        public bool IsEmailActive { get; set; }

        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0 را وارد نمایید}")]
        [MaxLength(40, ErrorMessage = " نمی تواند بیش از {1} کاراکتر باشد {0}")]
        public string Mobile { get; set; }

        [Display(Name = "کد فعال سازی تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0 را وارد نمایید}")]
        [MaxLength(40, ErrorMessage = " نمی تواند بیش از {1} کاراکتر باشد {0}")]
        public string MobileActiveCode { get; set; }

        [Display(Name = "موبایل فعال / غیرفعال")]
        public bool IsMobileActive { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0 را وارد نمایید}")]
        [MaxLength(100, ErrorMessage = " نمی تواند بیش از {1} کاراکتر باشد {0}")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "تصویر پروفایل")]
        [MaxLength(120, ErrorMessage = " نمی تواند بیش از {1} کاراکتر باشد {0}")]
        public string Avatar { get; set; }

        [Display(Name = "نام")]
        [MaxLength(100, ErrorMessage = " نمی تواند بیش از {1} کاراکتر باشد {0}")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [MaxLength(100, ErrorMessage = " نمی تواند بیش از {1} کاراکتر باشد {0}")]
        public string LastName { get; set; }

        [Display(Name = "مسدود شده / نشده")]
        public bool IsBlocked { get; set; }

        #endregion

        #region relations

        public ICollection<ContactUs> ContactUs { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        public ICollection<TicketMessage> TicketMessages { get; set; }
        public ICollection<Seller> Sellers { get; set; }
        public ICollection<ProductDiscountUsage> ProductDiscountUsagest { get; set; }

        #endregion

    }
}
