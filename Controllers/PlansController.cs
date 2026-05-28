<<<<<<< HEAD
﻿using GymMangement.DAL.Data.Models;
=======
﻿using GemMangement.DbContexts;
>>>>>>> c3cde5e44ef18a437e8f2711eed49a96e6f21de9
using GymMangement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymMangement.Controllers
{
    public class PlansController : Controller
    {
<<<<<<< HEAD
        private readonly IGenericRepository<Plan> _planRepository;
        public PlansController(IGenericRepository<Plan> planRepository)
        {
            this._planRepository = planRepository;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await _planRepository.GetAllAsync(ct:ct);
=======
        private readonly IPlanRepository planRepository;
        public PlansController(IPlanRepository planRepository)
        {
            this.planRepository = planRepository;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await planRepository.GetAllAsync(ct:ct);
>>>>>>> c3cde5e44ef18a437e8f2711eed49a96e6f21de9
            return View(plans);
        }

        public async Task<IActionResult> Details(int id ,CancellationToken ct)
        {
<<<<<<< HEAD
            var plan = await _planRepository.GetByIdAsync(id,ct);
=======
            var plan = await planRepository.GetByIdAsync(id,ct);
>>>>>>> c3cde5e44ef18a437e8f2711eed49a96e6f21de9

            if (plan is null)
            {
                return RedirectToAction(nameof(Index));

            }
            else
            {
                return View(plan);
            }
        }
    }
}
