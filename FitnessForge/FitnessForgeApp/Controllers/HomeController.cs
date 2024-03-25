using FitnessForgeAdmin.Models.Contexts;
using FitnessForgeApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FitnessForgeApp.Controllers
{
    public class HomeController : Controller
    {
        private SignInManager<ApplicationUser> _signInManager;
        private UserMealContext db;
        private UserManager<ApplicationUser> _userManager;

        public HomeController(SignInManager<ApplicationUser> signInManager, UserMealContext userMealContext, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            db = userMealContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                try
                {
                    var currentUser = await _userManager.GetUserAsync(User);

                    if (currentUser == null)
                    {
                        ViewData["ErrorMessage"] = "A felhasználó nem található";
                        return View();
                    }

                    var dailyIntake = await db.dailyIntakes
                        .FirstOrDefaultAsync(d => d.Date == DateTime.Today && d.UserId == currentUser.Id);

                    if (dailyIntake == null)
                    {
                        DailyIntake intake = new DailyIntake { Date = DateTime.Today, User = currentUser, UserId = currentUser.Id };
                        await db.dailyIntakes.AddAsync(intake);
                        currentUser.DailyIntakes.Add(intake);
                        await _userManager.UpdateAsync(currentUser);
                    }

                    return RedirectToAction("Home", "User");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hiba: {ex.Message}");
                    ViewData["ErrorMessage"] = "Hiba lépett fel a napi bevitel kezelésénél";
                    return RedirectToAction("Index", "User");
                }
            }
            return View();
        }
    }
}
