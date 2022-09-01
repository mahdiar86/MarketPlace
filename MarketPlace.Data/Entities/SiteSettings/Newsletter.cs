using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketPlace.Data.Entities.Common;

namespace MarketPlace.Data.Entities.SiteSettings
{
    public class Newsletter : BaseEntity
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = " نمی تواند بیش از {1} کاراکتر باشد {0}")]
        public string Email { get; set; }
    }
}
