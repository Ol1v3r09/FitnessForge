using FitnessForgeAdmin.Models.Contexts;
using FitnessForgeApp.Models;
using FitnessForgeApp.Models.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FitnessForgeApp.Controllers
{
    //Az ideiglenes Home oldal
    public class HomeController : Controller
    {
        SignInManager<ApplicationUser> SignInManager { get; set; }
        UserMealContext db { get; set; }
        UserManager<ApplicationUser> UserManager { get; set; }
        public HomeController(SignInManager<ApplicationUser> signInManager, UserMealContext userMealContext, UserManager<ApplicationUser> userManager)
        {
            SignInManager = signInManager;
            db = userMealContext;
            UserManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            //Ha nincs a User-nek a mai napra DailyIntake-je akkor létrehozunk egyet ui.: EZT KÉSÕBB MAJD ÁT KELL ÍRNI
            if (SignInManager.IsSignedIn(User))
            {
                try
                {
                    var currentUser = await UserManager.GetUserAsync(User);

                    if (currentUser == null)
                    {
                        ViewData["ErrorMessage"] = "A felhaználó nem található";
                        return View();
                    }

                    var dailyIntake = await db.dailyIntakes.FirstOrDefaultAsync(d => d.Date == DateTime.Today && d.UserId == currentUser.Id);

                    if (dailyIntake == null)
                    {
                        DailyIntake intake = new DailyIntake { Date = DateTime.Today, User = currentUser, UserId = currentUser.Id };
                        await db.dailyIntakes.AddAsync(intake);
                        currentUser.DailyIntakes.Add(intake);
                        await UserManager.UpdateAsync(currentUser);
                    }
                    return View();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hiba: {ex.Message}");

                    ViewData["ErrorMessage"] = "Hiba lépett fel a napi bevitel kezelésénél";
                    return View();
                }
            }
            return View();
        }
    }
}
