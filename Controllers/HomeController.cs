using Microsoft.AspNetCore.Mvc;
using SkillHub.Interfaces;
using SkillHub.Models;
using System.Diagnostics;
using System.Data;
using Dapper;

namespace SkillHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceRepository _serviceRepo;
        private readonly IDbConnection _db;

        public HomeController(IServiceRepository serviceRepo, IDbConnection db)
        {
            _serviceRepo = serviceRepo;
            _db = db;
        }

        public async Task<IActionResult> Index(string query, int? categoryId, string sort)
        {
            ViewBag.CurrentSort = sort;
            ViewBag.CurrentQuery = query;
            ViewBag.CurrentCategory = categoryId;
            
            var categorySql = "SELECT * FROM Categories";
            ViewBag.Categories = await _db.QueryAsync<Category>(categorySql);

            var services = await _serviceRepo.SearchServicesAsync(query, categoryId, sort);
            return View(services);
        }

        public async Task<IActionResult> Details(int id)
        {
            var service = await _serviceRepo.GetByIdAsync(id);
            if (service == null) return NotFound();
            return View(service);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
