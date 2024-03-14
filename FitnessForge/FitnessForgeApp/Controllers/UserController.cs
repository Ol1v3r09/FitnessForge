using FitnessForgeAdmin.Models.Contexts;
using FitnessForgeApp.Models;
using FitnessForgeApp.Models.ViewModels;
using Humanizer;
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
            List<Product> products = db.products.ToList();
        }

        [HttpGet]
        public async Task<IActionResult> Home()
        {
            try
            {
                ApplicationUser currentUser = await _userManager.GetUserAsync(User);

                List<DetailsViewModel> allMeals = new List<DetailsViewModel>();

                List<MealType> mealTypes = await db.mealTypes.ToListAsync();
                var dailyIntake = await db.dailyIntakes.Where(d => d.UserId == currentUser.Id && d.Date == DateTime.Today).FirstOrDefaultAsync();
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
                                        var unit = await db.foods.Where(x => x.Id == food.Id).Select(x => x.Unit).FirstAsync();
                                        double foodAmount = food.Amount;
                                        if (unit.Name.ToLower().Contains("gramm") && unit.Name != "Gramm")
                                        {
                                            foodAmount = UnitConverter.ConvertMass(foodAmount, unit.Name, "Gramm");
                                        }
                                        else if (unit.Name.ToLower().Contains("liter") && unit.Name != "Milliliter")
                                        {
                                            foodAmount = UnitConverter.ConvertVolume(foodAmount, unit.Name, "Milliliter");
                                        }
                                        var amount = (foodAmount / 100) * db.meals.Where(x => x.FoodId == food.Id && x.MealTypeId == mealType.Id && x.DailyIntake == dailyIntake).First().Amount;
                                        details.Calorie += Math.Round(fp.Product.Calorie * amount, 2);
                                        details.Carbohydrate += Math.Round(fp.Product.Carbohydrate * amount, 2);
                                        details.Protein += Math.Round(fp.Product.Protein * amount, 2);
                                        details.Fat += Math.Round(fp.Product.Fat * amount, 2);
                                        details.Fiber += Math.Round(fp.Product.Fiber * amount, 2);
                                        details.Salt += Math.Round(fp.Product.Salt * amount, 2);
                                        details.SaturatedFat += Math.Round(fp.Product.SaturatedFat * amount, 2);
                                        details.Sugar += Math.Round(fp.Product.Sugar * amount, 2);
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
                    if (user.Weight > user.WeightGoal)
                        currentUser.WeeklyWeightGoal = user.WeeklyWeightGoal * -1;
                    else if (user.Weight < user.WeightGoal)
                        currentUser.WeeklyWeightGoal = user.WeeklyWeightGoal;
                    else
                        currentUser.WeeklyWeightGoal = 0;
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
