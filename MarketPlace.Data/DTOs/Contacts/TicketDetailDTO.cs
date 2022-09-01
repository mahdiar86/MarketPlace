using System.Collections.Generic;
using MarketPlace.Data.Entities.Contacts;

namespace MarketPlace.Data.DTOs.Contacts
{
    public class TicketDetailDTO
    {
        public Ticket Ticket { get; set; }

        public List<TicketMessage> TicketMessage { get; set; }
    }
}
