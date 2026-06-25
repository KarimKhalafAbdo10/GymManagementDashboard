using GymMangement.BLL.Common;
using GymMangement.BLL.Services.Interfaces;
using GymMangement.BLL.ViewModels.SessionViewModels;
using GymMangement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymMangement.PL.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService sessionService;

        public SessionsController(ISessionService sessionService)
        {
            this.sessionService = sessionService    ;
        }
        #region Create Session
        [HttpGet]
        public async Task<IActionResult> Create()
        {
           await PopulateDropDownListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
           

            if (!ModelState.IsValid)
            {
                await PopulateDropDownListAsync();
                return View(model);
            }

            var result = await sessionService.CreateSessionAsync(model, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Session created successfully.";
                return RedirectToAction(nameof(Index));
            }

            
            TempData["ErrorMessage"] = result.Error;
            await PopulateDropDownListAsync();
            return View(model);
        }
        #endregion

        #region All Sessions
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var sessions = await sessionService.GetAllSessionsAsync(ct);
            return View(sessions);
        }
        #endregion


        #region Session Details
        [HttpGet]
        public async Task<IActionResult> Details(int Id , CancellationToken ct)
        {
            var session = await sessionService.GetSessionById(Id, ct);
            if (session.Success) return View(session.Value);
            else
            {
                TempData["ErrorMessage"] = session.Error;
            }
            return RedirectToAction(nameof(Index));

        }

        #endregion

        #region Edit Session
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var session = await sessionService.GetSessionToUpdateAsync(id,ct);
            if (session.Success)
            {
                ViewBag.Trainers = new SelectList(await sessionService.GetTrainerDropDownListAsync(), "Id", "Name");
                return View(session.Value);
            }
            TempData["ErrorMessage"] = session.Error;
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Edit( int id , EditSessionViewModel model,CancellationToken ct)
        {
            if (!ModelState.IsValid) {


                ViewBag.Trainers = new SelectList(await sessionService.GetTrainerDropDownListAsync(), "Id", "Name");

                return View(model); 
            
            }

            var result =await sessionService.UpdateSessionAsync(id,model,ct);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Session Update Successfullly";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"]=result.Error;
                ViewBag.Trainers = new SelectList(await sessionService.GetTrainerDropDownListAsync(), "Id", "Name");

                return View(model);
            }
        }
        #endregion

        #region Delete Session

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CacheProfile ct)
        {
            var session= await sessionService.GetSessionById(id);
            if (session.Success) return View(session.Value);
            TempData["ErroeMessage"]=session.Error;
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var session = await sessionService.DeleteSessionAsync(id);

            TempData[session.Success ? "SuccessMessage" : "ErrorMessage"] = session.Success ? "Session Deleted Successfully" : session.Error;
            return RedirectToAction(nameof(Index));
        }
        #endregion
        private async Task PopulateDropDownListAsync()
        {
            ViewBag.Trainers = new SelectList(await sessionService.GetTrainerDropDownListAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await sessionService.GetCategoryDropDownListAsync(), "Id", "CategoryName");
        }
    }
}
