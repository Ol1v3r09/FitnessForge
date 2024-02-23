using FitnessForgeAdmin.Models.Contexts;
using FitnessForgeApp.Models;
using FitnessForgeApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FitnessForgeApp.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        UserMealContext db;
        UserManager<ApplicationUser> _userManager { get; set; }

        public UserController(UserMealContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Home()
        {
            try
            {
                ApplicationUser currentUser = await _userManager.GetUserAsync(User);

                List<DetailsViewModel> allMeals = new List<DetailsViewModel>();

                List<MealType> mealTypes = await db.mealTypes.ToListAsync();

                var dailyIntake = await db.dailyIntakes.FirstOrDefaultAsync(d => d.UserId == currentUser.Id && d.Date == DateTime.Today);
                if (dailyIntake != null)
                {
                    foreach (var mealType in mealTypes)
                    {
                        DetailsViewModel details = new DetailsViewModel();

                        var allFoodsToday = await (from m in db.meals
                                           where m.IntakeId == dailyIntake.Id && m.MealTypeId == mealType.Id
                                           select m.Food).ToListAsync();

                        if (allFoodsToday != null)
                        {
                            foreach (var food in allFoodsToday)
                            {
                                if (food != null)
                                {
                                    var foodProducts = await (from fp in db.foodsHasProducts
                                                              where fp.FoodId == food.Id
                                                              select fp).ToListAsync();

                                    foreach (var fp in foodProducts)
                                    {
                                        details.Calorie += fp.Product.Calorie;
                                        details.Carbohydrate += fp.Product.Carbohydrate;
                                        details.Protein += fp.Product.Protein;
                                        details.Fat += fp.Product.Fat;
                                    }
                                }
                            }
                        }

                        allMeals.Add(details);
                    }
                }

                ViewData["mealTypes"] = mealTypes;
                return View(allMeals);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba: {ex.Message}");

                ViewData["ErrorMessage"] = "Hiba lépett fel az étkezések lekérdezése közben";
                return View();
            }

        }

        [HttpGet]
        public async Task<IActionResult> CreateDetails()
        {
            try
            {
                var allNutrients = await db.nutrientGoals.ToListAsync();
                var allActivities = await db.activityLevels.ToListAsync();

                ViewData["allActivities"] = allActivities;
                ViewData["Allnutrients"] = allNutrients;

                var currentUser = await _userManager.GetUserAsync(User);
                return View(currentUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba: {ex.Message}");

                ViewData["ErrorMessage"] = "Hiba az aktivitási szintek és a tápanyag célok lekérdezése közben";
                return View();
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateDetails(ApplicationUser user)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);

                if (currentUser != null)
                {
                    currentUser.DateOfBirth = user.DateOfBirth;
                    currentUser.Sex = user.Sex;
                    currentUser.Weight = user.Weight;
                    currentUser.Height = user.Height;
                    currentUser.WeightGoal = user.WeightGoal;
                    currentUser.WeeklyWeightGoal = user.WeeklyWeightGoal;
                    currentUser.ActivityId = user.ActivityId;
                    currentUser.NutrientId = user.NutrientId;

                    var result = await _userManager.UpdateAsync(currentUser);

                    if (!result.Succeeded)
                    {
                        Console.WriteLine($"Hiba a frissítésnél: {result.Errors}");

                        ViewData["ErrorMessage"] = "Hiba a felhasználó fríssítése közben";
                        return RedirectToAction("Index", "Home");
                    }
                }

                return RedirectToAction("CreateDetails");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba: {ex.Message}");

                ViewData["ErrorMessage"] = "Hiba az adatok frissítése közben";
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpGet]
        public async Task<IActionResult> ManageDetails()
        {
            try
            {
                var allNutrients = await db.nutrientGoals.ToListAsync();
                var allActivities = await db.activityLevels.ToListAsync();

                ViewData["allActivities"] = allActivities;
                ViewData["allNutrients"] = allNutrients;

                var currentUser = await _userManager.GetUserAsync(User);
                return View(currentUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba: {ex.Message}");

                ViewData["ErrosMessage"] = "Hiba lépett fel a felhasználó, a tápanyag célok és a aktivitási szintek lekérésekor";
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public async Task<IActionResult> ManageDetails(ApplicationUser user)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);

                if (currentUser != null)
                {
                    currentUser.DateOfBirth = user.DateOfBirth;
                    currentUser.Sex = user.Sex;
                    currentUser.Weight = user.Weight;
                    currentUser.Height = user.Height;
                    currentUser.WeightGoal = user.WeightGoal;
                    currentUser.WeeklyWeightGoal = user.WeeklyWeightGoal;
                    currentUser.ActivityId = user.ActivityId;
                    currentUser.NutrientId = user.NutrientId;

                    var result = await _userManager.UpdateAsync(currentUser);

                    if (!result.Succeeded)
                    {
                        Console.WriteLine($"Hiba a frissítésnél: {result.Errors}");
                        ViewData["ErrorMessage"] = "Hiba a felhasználó fríssítése közben";
                        return RedirectToAction("Index", "Home");
                    }
                }
                return RedirectToAction("ManageDetails");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba: {ex.Message}");

                return RedirectToAction("Index", "Home");
            }

        }
    }
}
