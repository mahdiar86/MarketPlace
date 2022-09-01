using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Data.DTOs.Site
{
    public class CapchaViewModel
    {
        [Required]
        public string Captcha { get; set; }
    }
}
