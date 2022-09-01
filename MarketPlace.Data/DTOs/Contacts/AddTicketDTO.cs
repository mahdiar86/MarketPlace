using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketPlace.Data.Entities.Account;
using MarketPlace.Data.Entities.Contacts;

namespace MarketPlace.Data.DTOs.Contacts
{
    public class AddTicketDTO
    {

        [Display(Name = "عنوان تیکت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "موضوع")]
        public TicketSection TicketSection { get; set; }

        [Display(Name = "اولویت")]
        public TicketPriority TicketPriority { get; set; }

        [Display(Name = "متن تیکت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Text { get; set; }

    }

    public enum AddTicketResult
    {
        Error,Success
    }

}
