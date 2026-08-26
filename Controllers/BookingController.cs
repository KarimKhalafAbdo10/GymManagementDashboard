using GymMangement.BLL.Services.Classes;
using GymMangement.BLL.Services.Interfaces;
using GymMangement.BLL.ViewModels.BookingViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymMangement.PL.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService bookingService;

        public BookingController(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        
            => View(await bookingService.GetAllSessionsAsync(ct));



        public async Task<IActionResult> GetMembersForUpcomingSession(int id, CancellationToken ct)
       => View(await bookingService.GetMemeberForUpcomingBySessionIdAsync(id,ct));
        public async Task<IActionResult> GetMembersForOngoingSession(int id, CancellationToken ct)
       => View(await bookingService.GetMemeberForOngoingBySessionIdAsync(id,ct));

        public async Task<IActionResult> Create(int id,CancellationToken ct)
        {
            var memebers =await bookingService.GetMemebersForDropDownListAsync(id);
            ViewBag.Members = new SelectList(memebers,"Id","Name");
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingViewModel model, CancellationToken ct)
        {
            var result = await bookingService.CreateNewBooking(model, ct);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] =
                result.Success ? "Booking created successfully." : result.Error;
            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = model.SessionId });
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int memberId, int sessionId, CancellationToken ct)
        {
            var result = await bookingService.CancelBookingAsync(memberId, sessionId, ct);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] =
                result.Success ? "Booking cancelled successfully." : result.Error;
            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = sessionId });
        }

        [HttpPost]
        public async Task<IActionResult> Attended(int memberId, int sessionId, CancellationToken ct)
        {
            var result = await bookingService.MarkAttendAsync(memberId, sessionId, ct);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] =
                result.Success ? "Attendance recorded." : result.Error;
            return RedirectToAction(nameof(GetMembersForOngoingSession), new { id = sessionId });
        }


    }
}
