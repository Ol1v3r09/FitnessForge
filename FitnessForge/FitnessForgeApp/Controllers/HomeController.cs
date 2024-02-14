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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
