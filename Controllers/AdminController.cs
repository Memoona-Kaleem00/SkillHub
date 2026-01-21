using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHub.Interfaces;

namespace SkillHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepo;

        public AdminController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<IActionResult> Dashboard()
        {
            var totalUsers = await _userRepo.GetTotalUsersCountAsync();
            var totalProviders = await _userRepo.GetTotalProvidersCountAsync();
            
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalProviders = totalProviders;

            return View();
        }

        public async Task<IActionResult> ApproveProviders()
        {
            var pendingProviders = await _userRepo.GetPendingProvidersAsync();
            return View(pendingProviders);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveProvider(string id)
        {
            await _userRepo.ApproveProviderAsync(id);
            return RedirectToAction(nameof(ApproveProviders));
        }
    }
}
