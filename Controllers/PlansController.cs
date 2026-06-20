
using GymMangement.BLL.Services.Interfaces;
using GymMangement.BLL.ViewModels.PlanViewModels;
using GymMangement.DAL.Data.Models;
using GymMangement.DAL.Repositories.Interfaces;

    


using Microsoft.AspNetCore.Mvc;

namespace GymMangement.Controllers
{
    public class PlansController : Controller
    {
        private readonly IPlanService planService;

        public PlansController(IPlanService planService)
        {
            this.planService = planService;
        }
       

       
      
        
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await planService.GetAllPlansAsync(ct:ct);
            return View(plans);
        }

        public async Task<IActionResult> Details(int id ,CancellationToken ct)
        {

            var plan = await planService.GetPlanById(id,ct);

           

            if (plan is null)
            {
                return RedirectToAction(nameof(Index));

            }
            else
            {
                return View(plan);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var plan = await planService.PlanToUpdateAsync(id, ct);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan can't be edited(Not Found ,Inactive or has Membership)";
                 RedirectToAction(nameof(Index));
            }
            return View(plan);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id ,UpdatePlanViewModel model,CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await planService.UpdatePlanAsync(id, model, ct);
            if (result)
            
                TempData["SuccessMessage"] = "Plan updated successfully";

            else
                TempData["ErrorMessage"] = "Plan can't be updated(Not Found ,Inactive or has Membership)";
            return RedirectToAction(nameof(Index));
  

        }
        [HttpPost]
        public async Task<IActionResult> Activate(int id, CancellationToken ct)
        {
            var result = await planService.ToggleActivationAsync(id, ct);
            if (result)
                TempData["SuccessMessage"] = "Plan Status Changed successfully";
            else
                TempData["ErrorMessage"] = "Plan can't be Activated/Deactivated(Not Found or has Membership)";
            return RedirectToAction(nameof(Index));
        }
    }
}
