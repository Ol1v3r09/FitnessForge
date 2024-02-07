using FitnessForgeAdmin.Models.Contexts;
using FitnessForgeApp.Models;
using FitnessForgeApp.Models.Contexts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FitnessForgeApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, UserMealContext a, UserExerciseContext b)
        {
            _logger = logger;
            a.Database.EnsureCreated();
        }

        public IActionResult Index()
        {
            return View();
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
