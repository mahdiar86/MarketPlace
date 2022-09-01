using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Data.DTOs.Contacts;
using MarketPlace.Data.Entities.Contacts;
using MarketPlace.Data.Repository;
using System.Linq;
using System.Threading.Tasks;
using MarketPlace.Data.DTOs.Paging;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Application.Services.Implementation
{
    public class ContactService : IContactService
    {
        #region Constrcutor

        private readonly IGenericRepository<ContactUs> _contactUsRepository;
        private readonly IGenericRepository<Ticket> _ticketRepository;
        private readonly IGenericRepository<TicketMessage> _ticketMessageRepository;

        public ContactService(IGenericRepository<ContactUs> contactUsRepository, IGenericRepository<Ticket> ticketRepository, IGenericRepository<TicketMessage> ticketMessageRepository)
        {
            _contactUsRepository = contactUsRepository;
            _ticketRepository = ticketRepository;
            _ticketMessageRepository = ticketMessageRepository;
        }
        #endregion

        #region Contact Us

        public async Task CreateContactUs(CreateContactUsDTO contact, string userIp, long? userId)
        {
            var newContact = new ContactUs
            {
                UserId = userId != null && userId.Value != 0 ? userId.Value : (long?)null,
                FullName = contact.FullName,
                Email = contact.Email,
                Text = contact.Text,
                MobileNumber = contact.MobileNumber,
                UserIp = userIp
            };

            await _contactUsRepository.AddEntity(newContact);
            await _contactUsRepository.SaveChanges();

        }

        #endregion

        #region Ticket

        public async Task<AddTicketResult> AddUserTicket(AddTicketDTO ticket,long userId)
        {
            if (string.IsNullOrEmpty(ticket.Text))
                return AddTicketResult.Error;
            
            var newTicket = new Ticket()
            {
                OwnerId = userId,
                IsReadByOwner = true,
                TicketPriority = ticket.TicketPriority,
                Title = ticket.Title,
                TicketSection = ticket.TicketSection,
                TicketState = TicketState.UnderProgress
            };

            await _ticketRepository.AddEntity(newTicket);
            await _ticketRepository.SaveChanges();

            var newMessage = new TicketMessage()
            {
                TicketId = newTicket.Id,
                Text = ticket.Text,
                SenderId = userId
            };

            await _ticketMessageRepository.AddEntity(newMessage);
            await _ticketMessageRepository.SaveChanges();

            return AddTicketResult.Success;
        }

        public async Task<FilterTicketDTO> FilterTickets(FilterTicketDTO filter)
        {
            var query = _ticketRepository.GetQuery().AsQueryable();

            var x = query.ToList();

            switch (filter.FilterTicketState)
            {
                case FilterTicketState.All:
                    break;
                case FilterTicketState.Deleted:
                    query = query.Where(s => s.IsDelete);
                    break;
                case FilterTicketState.NotDeleted:
                    query = query.Where(s => !s.IsDelete);
                    break;
            }

            switch (filter.OrderBy)
            {
                case FilterTicketOrder.CreateDate_ASC:
                    query = query.OrderBy(s => s.CreateDate);
                    break;
                case FilterTicketOrder.CreateDate_DSC:
                    query = query.OrderByDescending(s => s.CreateDate);
                    break;
            }

            if (filter.TicketSection != null)
                query = query.Where(s => s.TicketSection == filter.TicketSection.Value);

            if (filter.TicketPriority != null)
                query = query.Where(s => s.TicketPriority == filter.TicketPriority.Value);

            if (filter.UserId != null && filter.UserId != 0)
                query = query.Where(s => s.OwnerId == filter.UserId.Value);

            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(t => t.Title.Contains(filter.Title));

            //if (!string.IsNullOrEmpty(filter.Title))
            //query = query.Where(s => EF.Functions.Like(s.Title, $"%{filter.Title}%"));

            var p = query.ToList();

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntities = query.ToList().PagingList(pager).ToList();

            return filter.SetPaging(pager).SetTickets(allEntities);
        }

        public async Task<TicketDetailDTO> GetTicketForShow(long ticketId, long userId)
        {
            var ticket = await _ticketRepository.GetQuery().AsQueryable().Include(t => t.Owner).SingleOrDefaultAsync(s => s.Id == ticketId);

            if (ticket == null || ticket.OwnerId != userId)
                return null;

            return new TicketDetailDTO
            {
                Ticket = ticket,
                TicketMessage = await _ticketMessageRepository.GetQuery().Include(c=>c.Sender).AsQueryable().Where(t=>t.TicketId == ticketId && !t.IsDelete).OrderByDescending(d=>d.CreateDate).ToListAsync()
            };
        }

        public async Task<AnswerTicketResult> AnswerTicket(AnswerTicketDTO answer, long userId)
        {
            var ticket = await _ticketRepository.GetEntityById(answer.Id);

            if (ticket == null)
                return AnswerTicketResult.NotFound;

            if (ticket.OwnerId != userId)
                return AnswerTicketResult.NotForUser;

            var ticketMessage = new TicketMessage
            {
                TicketId = ticket.Id,
                SenderId = userId,
                Text = answer.Text
            };

            await _ticketMessageRepository.AddEntity(ticketMessage);
            await _ticketMessageRepository.SaveChanges();

            ticket.IsReadByAdmin = false;
            ticket.IsReadByOwner = true;

            await _ticketRepository.SaveChanges();

            return AnswerTicketResult.Success;
        }

        #endregion

        #region Dispose

        public async ValueTask DisposeAsync()
        {
            await _contactUsRepository.DisposeAsync();
        }

        #endregion

    }
}
