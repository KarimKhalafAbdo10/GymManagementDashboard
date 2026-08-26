using GemMangement.Controllers;
using GymMangement.BLL.ViewModels.AccountViewModels;
using GymMangement.DAL.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymMangement.PL.Controllers
{
    public class AccountController : Controller


    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
       
        public async Task<IActionResult> Login(LoginViewModel model,CancellationToken ct=default)
        {
            if(!ModelState.IsValid) return View(model);

            var user =await _userManager.FindByEmailAsync(model.Email);
            if(user==null)
            {
                ModelState.AddModelError("InValid Login", "Invalid Email Or Password .");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
            if (result.Succeeded)
            {
                _logger.LogInformation($"User{user.UserName} Signed In Successfully ");
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else if(result.IsLockedOut)
            {
                _logger.LogWarning($"User{user.UserName} Account Is Locked Out ");
                ModelState.AddModelError("Locked Out", "Your Account Is Locked Out, Try Again Later .");
                return View(model);
            }
            else
            {
                _logger.LogWarning($"User{user.UserName} Failed To Sign In ");
                ModelState.AddModelError("InValid Login", "Invalid Email Or Password .");
                return View(model);
            }
            
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
           await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }

        public IActionResult AccessDenied() => View();

    }
}
