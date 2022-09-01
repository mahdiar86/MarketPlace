using System.Collections.Generic;
using MarketPlace.Data.DTOs.Paging;
using MarketPlace.Data.Entities.Contacts;

namespace MarketPlace.Data.DTOs.Contacts
{
    public class FilterTicketDTO : BasePaging
    {
        public string Title { get; set; }
        
        public long? UserId { get; set; }

        public FilterTicketState FilterTicketState { get; set; }

        public FilterTicketOrder OrderBy { get; set; }

        public TicketSection? TicketSection { get; set; }

        public TicketPriority? TicketPriority { get; set; }

        public List<Ticket> Tickets { get; set; }


        public FilterTicketDTO SetTickets(List<Ticket> tickets)
        {
            this.Tickets = tickets;
            return this;
        }

        public FilterTicketDTO SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            this.PageCount = paging.PageCount;
            return this;
        }
    }

    public enum FilterTicketState
    {
        All,
        NotDeleted,
        Deleted
    }

    public enum FilterTicketOrder
    {
        CreateDate_DSC,
        CreateDate_ASC,

    }
}
