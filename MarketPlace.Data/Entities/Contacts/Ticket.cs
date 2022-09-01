using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MarketPlace.Data.Entities.Account;
using MarketPlace.Data.Entities.Common;

namespace MarketPlace.Data.Entities.Contacts
{
    public class Ticket : BaseEntity
    {
        public long OwnerId { get; set; }

        [Display(Name = "عنوان تیکت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "وضعیت تیکت")] 
        public TicketState TicketState { get; set; }

        [Display(Name = "موضوع")]
        public TicketSection TicketSection { get; set; }

        [Display(Name = "اولویت")]
        public TicketPriority TicketPriority { get; set; }

        public bool IsReadByOwner { get; set; }

        public bool IsReadByAdmin { get; set; }


        public ICollection<TicketMessage> TicketMessages { get; set; }

        public User Owner { get; set; }
    }

    public enum TicketSection
    {
        [Display(Name = "پشتیبانی")]
        Support,
        [Display(Name = "بخش فنی")]
        Technical,
        [Display(Name = "امور مشتریان")]
        CustomersService
    }

    public enum TicketPriority
    {
        [Display(Name = "کم")]Low,
        [Display(Name = "متوسط")] Medium,
        [Display(Name = "زیاد")] High
    }

    public enum TicketState
    {
        [Display(Name = "درحال بررسی")]
        UnderProgress,
        [Display(Name = "پاسخ داده شده")]
        Answered,
        [Display(Name = "بسته شده")]
        Closed
    }

}
