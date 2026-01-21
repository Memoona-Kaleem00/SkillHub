using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkillHub.Interfaces;
using SkillHub.Models;

namespace SkillHub.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerController(IBookingRepository bookingRepo, UserManager<ApplicationUser> userManager)
        {
            _bookingRepo = bookingRepo;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> BookingHistory()
        {
            var userId = _userManager.GetUserId(User);
            var bookings = await _bookingRepo.GetBookingsByCustomerAsync(userId);
            return View(bookings);
        }
    }
}
