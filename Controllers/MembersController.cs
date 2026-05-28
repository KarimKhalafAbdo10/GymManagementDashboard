using GymMangement.BLL.Services.Interfaces;
using GymMangement.BLL.ViewModels.MemberViewModels;
using GymMangement.DAL.Data.Models;
using GymMangement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymMangement.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService memberService;

        public MembersController(IMemberService memberService)
        {
            this.memberService = memberService;
        }

        #region Get ALL memebers
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await memberService.GetAllAsync(ct);

            return View(members);
        }

        #endregion


        #region Create Member
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult>  CreateMember(CreateMemeberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create), model);

            var result = await memberService.CreateMemberAsync(model, ct);
            if (result)
            {
                TempData["Success"] = "Member Created Successfully";
            }
            else
            {
                TempData["Error"] = "Member Creation Failed, Email or Phone already exists";
            }

            return RedirectToAction(nameof(Index));


        }

        #endregion
    }
}
