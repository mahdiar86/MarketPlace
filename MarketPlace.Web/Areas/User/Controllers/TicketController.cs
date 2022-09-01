using System.Threading.Tasks;
using MarketPlace.Application.Services.Interfaces;
using MarketPlace.Data.DTOs.Contacts;
using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Areas.User.Controllers
{
    public class TicketController : UserBaseController
    {
        #region Constrcutor

        private readonly IContactService _contactService;
        private readonly IUserService _userService;

        public TicketController(IContactService contactService, IUserService userService)
        {
            _contactService = contactService;
            _userService = userService;
        }

        #endregion

        #region List

        [HttpGet("Tickets")]
        public async Task<IActionResult> Index(FilterTicketDTO filter)
        {
            filter.UserId = User.GetUserId();
            filter.FilterTicketState = FilterTicketState.NotDeleted;
            filter.OrderBy = FilterTicketOrder.CreateDate_DSC;

            return View(await _contactService.FilterTickets(filter));
        }

        #endregion

        #region Add Ticket

        [HttpGet("AddTicket")]
        public async Task<IActionResult> AddTicket()
        {
            return View();
        }

        [HttpPost("AddTicket")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicket(AddTicketDTO ticket)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(ticket.Text))
                {
                    TempData[WarningMessage] = "لطفا متن پیام را وارد کنید";
                    return View();
                }

                var result = await _contactService.AddUserTicket(ticket, User.GetUserId());
                switch (result)
                {
                    case AddTicketResult.Error:
                        TempData[ErrorMessage] = "خطایی رخ داده است";
                        break;
                    case AddTicketResult.Success:
                        TempData[SuccessMessage] = "تیکت شما با موفقیت ثبت شد , پاسخ شما تا لحظاتی دیگر داده خواهد شد";
                        return RedirectToAction("Index");
                }
            }

            return View();
        }

        #endregion

        #region Show Ticket Detail

        [HttpGet("Tickets/{id}")]
        public async Task<IActionResult> TicketDetail(long id)
        {
            var ticket = await _contactService.GetTicketForShow(id, User.GetUserId());
            if (ticket == null)
                return NotFound();

            return View(ticket);
        }

        #endregion

        #region Answer Ticket

        [HttpPost("AnswerTicket"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AnswerTicket(AnswerTicketDTO answer)
        {
            if (string.IsNullOrEmpty(answer.Text))
            {
                TempData[WarningMessage] = "لطفا متن پیام خود را وارد کنید";
            }
            if (ModelState.IsValid)
            {
                var result = await _contactService.AnswerTicket(answer, User.GetUserId());
                switch (result)
                {
                    case AnswerTicketResult.NotForUser:
                        TempData[ErrorMessage] = "عدم دسترسی !";
                        return RedirectToAction("Index");
                    case AnswerTicketResult.NotFound:
                        TempData[ErrorMessage] = "اطلاعات مورد نظر یافت نشد !";
                        return RedirectToAction("Index");
                    case AnswerTicketResult.Success:
                        TempData[SuccessMessage] = "اطلاعات مورد نظر با موفقیت ثبت شد !";
                        break;
                }
            }

            return RedirectToAction("TicketDetail", "Ticket", new { area = "User", id = answer.Id });
        }

        #endregion

    }
}
