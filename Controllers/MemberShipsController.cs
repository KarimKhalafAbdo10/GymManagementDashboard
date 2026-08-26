using GymMangement.BLL.Services.Interfaces;
using GymMangement.BLL.ViewModels.MemberShipViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymMangement.PL.Controllers
{
    public class MemberShipsController : Controller
    {
        private readonly IMemberShipService _memberShipService;

        public MemberShipsController(IMemberShipService memberShipService)
        {
            _memberShipService = memberShipService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            return View(await _memberShipService.GetAllMemberShipsAsync(ct));
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {

            await PopulateDropDownList(ct);
            return View();



        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberShipViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownList(ct);
                return View(model);
            }

            var result = await _memberShipService.CreateMemberShipAsync(model, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "MemeberShip Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["Errormessage"] = result.Error;
            await PopulateDropDownList(ct);
            return View(model);


        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id,CancellationToken ct)
        {
            var result=await _memberShipService.DeleteMemberShipAsync(id);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Success ? "MemberShip Canceles" : result.Error;
            return RedirectToAction(nameof(Index));

        }
        private async Task PopulateDropDownList(CancellationToken ct)
        {
            var plans = await _memberShipService.GetPlanForDropDownAsync(ct) ?? [];
            var members = await _memberShipService.GetMemberForDropDownAsync(ct) ?? [];

            ViewBag.Plans = new SelectList(plans, "Id", "Name");
            ViewBag.Members = new SelectList(members, "Id", "Name");
        }

    }
}
