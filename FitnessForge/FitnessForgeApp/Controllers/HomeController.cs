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
            //Ha nincs a User-nek a mai napra DailyIntake-je akkor l�trehozunk egyet ui.: EZT K�S�BB MAJD �T KELL �RNI
            if (SignInManager.IsSignedIn(User))
            {
                try
                {
                    var currentUser = await UserManager.GetUserAsync(User);

                    if (currentUser == null)
                    {
                        ViewData["ErrorMessage"] = "A felhazn�l� nem tal�lhat�";
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

                    ViewData["ErrorMessage"] = "Hiba l�pett fel a napi bevitel kezel�s�n�l";
                    return View();
                }
            }
            return View();
        }
    }
}
