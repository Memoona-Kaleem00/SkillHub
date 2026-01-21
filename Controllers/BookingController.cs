using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkillHub.Interfaces;
using SkillHub.Models;
using System;
using System.Threading.Tasks;

namespace SkillHub.Controllers
{
    [Authorize(Roles = "Customer")]
    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly IServiceRepository _serviceRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(IBookingRepository bookingRepo, IServiceRepository serviceRepo, UserManager<ApplicationUser> userManager)
        {
            _bookingRepo = bookingRepo;
            _serviceRepo = serviceRepo;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int serviceId)
        {
            var service = await _serviceRepo.GetByIdAsync(serviceId);
            if (service == null) return NotFound();

            var booking = new Booking
            {
                ServiceId = serviceId,
                ProviderId = service.ProviderId,
                BookingDate = DateTime.Today.AddDays(1)
            };

            ViewBag.ServiceTitle = service.Title;
            ViewBag.Price = service.Price;
            ViewBag.ImageUrl = service.ImageUrl;

            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            // Refetch service to ensure data integrity (ProviderId, etc.)
            var service = await _serviceRepo.GetByIdAsync(booking.ServiceId);
            if (service == null) return NotFound();

            booking.CustomerId = user.Id;
            booking.ProviderId = service.ProviderId;
            booking.Status = "Pending";

            ModelState.Remove("CustomerId");
            ModelState.Remove("ProviderId");
            ModelState.Remove("Status");

            if (booking.BookingDate < DateTime.Today)
            {
                ModelState.AddModelError("BookingDate", "Date cannot be in the past.");
            }

            if (ModelState.IsValid)
            {
                await _bookingRepo.AddAsync(booking);
                return RedirectToAction("BookingHistory", "Customer");
            }

            ViewBag.ServiceTitle = service.Title;
            ViewBag.Price = service.Price;
            ViewBag.ImageUrl = service.ImageUrl;
            return View(booking);
        }
    }
}
