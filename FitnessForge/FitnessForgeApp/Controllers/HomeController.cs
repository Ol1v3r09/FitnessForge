using FitnessForgeAdmin.Models.Contexts;
using FitnessForgeApp.Models;
using FitnessForgeApp.Models.Contexts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FitnessForgeApp.Controllers
{
    //Az ideiglenes Home oldal
    public class HomeController : Controller
    {
        //Visszadja az Index View-t
        public IActionResult Index()
        {
            return View();
        }
    }
}
