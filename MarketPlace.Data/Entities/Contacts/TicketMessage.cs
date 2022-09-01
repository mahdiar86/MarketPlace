using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MarketPlace.Data.Entities.Account;
using MarketPlace.Data.Entities.Common;

namespace MarketPlace.Data.Entities.Contacts
{
    public class TicketMessage : BaseEntity
    {
        public long TicketId { get; set; }

        public long SenderId { get; set; }

        [Display(Name = "متن تیکت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Text { get; set; }

        public Ticket Ticket { get; set; }

        public User Sender { get; set; }

    }
}
