using MarketPlace.Data.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Data.Entities.SiteSettings
{
    public class Slider : BaseEntity
    {
        [Display(Name = "عنوان")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Header { get; set; }

        [Display(Name = "تصویر")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ImageName { get; set; }

        [Display(Name = "لینک اسلایدر")]
        public string Link { get; set; }

        [Display(Name = "فعال / غیر فعال ")]
        public bool IsActive { get; set; }

    }
}
