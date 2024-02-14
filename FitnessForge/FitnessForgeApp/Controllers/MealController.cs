using FitnessForgeAdmin.Models.Contexts;
using FitnessForgeApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessForgeApp.Controllers
{
    public class MealController : Controller
    {
        UserMealContext db;
        UserManager<ApplicationUser> userManager;

        public MealController(UserMealContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<IActionResult> EditIntake(MealType mealType)
        {
            var currentUser = await userManager.GetUserAsync(User);
            var foods = (from m in db.meals where m.DailyIntake.Date == DateTime.Today && currentUser.Id == m.DailyIntake.UserId && m.MealType.Name == mealType.Name select m.Food).DefaultIfEmpty().ToList();
            ViewBag.MealType = mealType;
            return View(foods);
        }

        public async Task<IActionResult> Delete(MealType mealType,int foodId)
        {
            var user = await userManager.GetUserAsync(User);
            var meal = await (from m in db.meals where m.MealType == mealType && m.DailyIntake.UserId == user.Id && m.DailyIntake.Date == DateTime.Today && m.FoodId == foodId select m).FirstOrDefaultAsync();
            if (meal != null)
            {
                db.meals.Remove(meal);
                db.SaveChanges();
            }
            return RedirectToAction("EditIntake");
        }
    }
}
