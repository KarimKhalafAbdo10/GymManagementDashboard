using GymMangement.BLL.Services.Interfaces;
using GymMangement.BLL.ViewModels.MemberViewModels;
using GymMangement.DAL.Data.Models;
using GymMangement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        #region memeber details


        public async Task<IActionResult> MemberDetails(int Id, CancellationToken ct)
        {
            var member = await memberService.MemberDetailsAsync(Id, ct);
            if (member == null)
            {

                    TempData["Error"] = "Member Not Found";
                    return RedirectToAction(nameof(Index));
            }
            return View(member);    

        }
        #endregion

        #region health record details       
   public async Task<IActionResult> HealthRecordDetails(int memberId, CancellationToken ct)
        {
            var healthRecord = await memberService.GetHealthRecordByMemberIdAsync(memberId, ct);
            if (healthRecord == null)
            {
                TempData["Error"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }
            
                return View(healthRecord);

            
        }
        #endregion

        #region EditMember
        [HttpGet]
        public async Task<IActionResult> EditMember(int memberId,CancellationToken ct)
        {
            var member= await memberService.EditMemberAsync(memberId, ct);
            if (member == null){

                TempData["error"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
        }
            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> EditMember( int memberId,EditMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await memberService.EditMemberDetailsAsync(memberId, model, ct);
            if (result)
            
                TempData["Success Message"] = "Member Updated Successfully";
            
            else
            
                TempData["error Message"] = "Failed to Update Member";
            
            return RedirectToAction(nameof(Index));
        }

        #endregion 

        #region Delete Member
        [HttpGet]
        public async Task<IActionResult> Delete(int Id, CancellationToken ct)
        {
            var member = await memberService.MemberDetailsAsync(Id, ct);
            if (member == null) 
            {
                TempData["ErrorMessage"] = "Memeber Not Found";
                return RedirectToAction(nameof(Index));
                
                
                
            }
            return View();

            


        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id, CancellationToken ct)
        {
            var member = await memberService.DeleteMemberAsync(id, ct);
            if (member)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Delete Member";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
