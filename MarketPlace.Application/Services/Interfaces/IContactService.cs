using MarketPlace.Data.DTOs.Contacts;
using System;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Interfaces
{
    public interface IContactService : IAsyncDisposable
    {
        Task CreateContactUs(CreateContactUsDTO contact, string userIp, long? userId);
        Task<AddTicketResult> AddUserTicket(AddTicketDTO ticket, long userId);
        Task<FilterTicketDTO> FilterTickets(FilterTicketDTO filter);
        Task<TicketDetailDTO> GetTicketForShow(long ticketId, long userId);
        Task<AnswerTicketResult> AnswerTicket(AnswerTicketDTO answer, long userId);
    }
}
