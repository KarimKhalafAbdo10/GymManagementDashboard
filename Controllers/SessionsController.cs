using GymMangement.BLL.Services.Interfaces;
using GymMangement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymMangement.PL.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService sessionService;

        public SessionsController(ISessionService sessionService)
        {
            this.sessionService = sessionService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
           var sessions = await sessionService.GetAllSessionsAsync(ct);
            return View(sessions);
        }
    }
}
