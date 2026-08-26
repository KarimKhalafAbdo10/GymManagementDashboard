using GymMangement.BLL.Services.Classes;
using GymMangement.BLL.Services.Interfaces;
using GymMangement.BLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymMangement.PL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TrainerController : Controller
    {
        private readonly ITrainerService trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            this.trainerService = trainerService;
        }

        #region Get ALL Trainers

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            return View(await trainerService.GetAllTrainersAsync(ct));
        }

        #endregion

        #region Create Trainer
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await trainerService.CreateTrainerAsync(model, ct);
            if(result.Success)
            {
                TempData["SuccessMessage"] = "Trainer created successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Trainer Failed create";
            return View(model);


            
        }

        #endregion


        #region Get Trainer Details

        public async Task<IActionResult> Details(int Id,CancellationToken ct)
        {
            var trainer = await trainerService.GetTrainerDetailsAsync(Id,ct);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);

        }

        #endregion


        #region Edit Trainer
        [HttpGet]
        public async Task<IActionResult> Edit(int Id,CancellationToken ct)
        {
            var trainer = await trainerService.GetTrainerToUpdateAsync(Id,ct);

            if(trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int Id, EditTrainerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await trainerService.UpdateTrainerDetailsAsync(Id, model, ct);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Trainer update failed.";
            return View(model);
        }
        #endregion


        #region Delete Trainer 
        [HttpGet]
        public async Task<IActionResult> Delete(int Id, CancellationToken ct)
        {
            var trainer = await trainerService.GetTrainerDetailsAsync(Id, ct);
            if(trainer is null)
            {
                
                    TempData["ErrorMessage"] = "Trainer not found.";
                    return RedirectToAction(nameof(Index));
                
               
            }
                return View();

        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute]int id, CancellationToken ct)
        {
            var trainer = await trainerService.DeleteTrainerAsync(id, ct);
            if (trainer)
                TempData["SuccessMessage"] = "Trainer deleted successfully.";
            TempData["ErrorMessage"] = "Failed To delete Trainer";
            return RedirectToAction(nameof(Index));
        }

        #endregion


    }
}
