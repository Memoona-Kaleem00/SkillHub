using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkillHub.Interfaces;
using SkillHub.Models;
using System.Security.Claims;

namespace SkillHub.Controllers
{
    [Authorize(Roles = "Provider")]
    public class ProviderController : Controller
    {
        private readonly IServiceRepository _serviceRepo;
        private readonly IBookingRepository _bookingRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProviderController(IServiceRepository serviceRepo, IBookingRepository bookingRepo, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _serviceRepo = serviceRepo;
            _bookingRepo = bookingRepo;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userId = _userManager.GetUserId(User);
            var services = await _serviceRepo.GetServicesByProviderAsync(userId);
            var bookings = await _bookingRepo.GetBookingsByProviderAsync(userId);
            ViewBag.TotalBookings = bookings.Count();
            return View(services);
        }

        public async Task<IActionResult> MyServices()
        {
             var userId = _userManager.GetUserId(User);
             var services = await _serviceRepo.GetServicesByProviderAsync(userId);
             return View(services);
        }

        [HttpGet]
        public IActionResult CreateService()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateService(Service service, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "services");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                service.ImageUrl = "/uploads/services/" + uniqueFileName;
            }

            service.ProviderId = _userManager.GetUserId(User);
            await _serviceRepo.AddAsync(service);
            return RedirectToAction(nameof(MyServices));
        }

        [HttpGet]
        public async Task<IActionResult> EditService(int id)
        {
            var service = await _serviceRepo.GetByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            if (service.ProviderId != userId)
            {
                return Unauthorized();
            }
            return View(service);
        }

        [HttpPost]
        public async Task<IActionResult> EditService(Service service, IFormFile? imageFile)
        {
            var originalService = await _serviceRepo.GetByIdAsync(service.ServiceId);
            if (originalService == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            if (originalService.ProviderId != userId)
            {
                return Unauthorized();
            }

            // Keep original ProviderId
            service.ProviderId = userId;
            
            // Handle image upload
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "services");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                service.ImageUrl = "/uploads/services/" + uniqueFileName;
            }
            else
            {
                // Keep existing image if no new one uploaded
                service.ImageUrl = originalService.ImageUrl;
            }
            
            await _serviceRepo.UpdateAsync(service);
            return RedirectToAction(nameof(MyServices));
        }

        public async Task<IActionResult> Bookings()
        {
            var userId = _userManager.GetUserId(User);
            var bookings = await _bookingRepo.GetBookingsByProviderAsync(userId);
            return View(bookings);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBookingStatus(int bookingId, string status)
        {
            await _bookingRepo.UpdateStatusAsync(bookingId, status);
            return RedirectToAction(nameof(Bookings));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _serviceRepo.GetByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (service.ProviderId != userId)
            {
                return Unauthorized();
            }

            await _serviceRepo.DeleteAsync(id);
            return RedirectToAction(nameof(MyServices));
        }
    }
}
