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

        public async Task<IActionResult> Home()
        {
            double[] d = new double[4];
            var currentUser = await _userManager.GetUserAsync(User);

            List<DetailsViewModel> allMeals = new List<DetailsViewModel>();
            DetailsViewModel all = new DetailsViewModel();

            List<MealType> mealTypes = db.mealTypes.ToList();
            List<Meal> meals = db.meals.ToList();
            List<FoodHasProduct> foodHasProducts = db.foodsHasProducts.ToList();
            List<Product> products = db.products.ToList();

            //Összes mai kalória kivétele
            var foods = (from m in meals where m.DailyIntake.Date == DateTime.Today && currentUser.Id == m.DailyIntake.UserId select m.Food).DefaultIfEmpty().ToList();
            if (foods != null)
            {
                foreach (var foodProduct in foodHasProducts)
                {
                    foreach (var item in foods)
                    {
                        if (item != null)
                        {
                            if (foodProduct.FoodId == item.Id)
                            {
                                var product = (from p in products where p.Id == foodProduct.ProductId select p).FirstOrDefault();
                                if (product != null)
                                {
                                    all.Calorie += product.Calorie;
                                    all.Carbohydrate += product.Carbohydrate;
                                    all.Protein += product.Protein;
                                    all.Fat += product.Fat;
                                }
                            }
                        }
                    }
                }
                allMeals.Add(all);
            }
            //Összes mai étkezésenkénti kalória kivétele
            foreach (var mealType in mealTypes)
            {
                DetailsViewModel u = new DetailsViewModel();
                foods = (from m in meals where m.DailyIntake.Date == DateTime.Today && currentUser.Id == m.DailyIntake.UserId && m.MealTypeId == mealType.Id select m.Food).DefaultIfEmpty().ToList();
                if (foods != null)
                {
                    foreach (var foodProduct in foodHasProducts)
                    {
                        foreach (var food in foods)
                        {
                            if (foodProduct.FoodId == food.Id)
                            {
                                var product = (from p in products where p.Id == foodProduct.ProductId select p).FirstOrDefault();
                                if (product != null)
                                {
                                    u.Calorie += product.Calorie;
                                    u.Carbohydrate += product.Carbohydrate;
                                    u.Protein += product.Protein;
                                    u.Fat += product.Fat;
                                }
                            }
                        }
                    }
                    allMeals.Add(u);
                }
            }

            ViewBag.MealTypes = mealTypes;
            return View(allMeals);
        }

        public async Task<IActionResult> CreateDetails()
        {
            var nutrients = await db.nutrientGoals.ToListAsync();
            var activities = await db.activityLevels.ToListAsync();

            ViewBag.Activities = activities;
            ViewBag.Nutrients = nutrients;
            var currentUser = await _userManager.GetUserAsync(User);
            return View(currentUser);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDetails(ApplicationUser user)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null) {
                currentUser.DateOfBirth = user.DateOfBirth;
                currentUser.Sex = user.Sex;
                currentUser.Weight = user.Weight;
                currentUser.Height = user.Height;
                currentUser.WeightGoal = user.WeightGoal;
                currentUser.WeeklyWeightGoal = user.WeeklyWeightGoal;
                currentUser.ActivityId = user.ActivityId;
                currentUser.NutrientId = user.NutrientId;
                var result = await _userManager.UpdateAsync(currentUser);
            }

            return RedirectToAction("CreateDetails");
        }

        public async Task<IActionResult> ManageDetails()
        {
            var nutrients = await db.nutrientGoals.ToListAsync();
            var activities = await db.activityLevels.ToListAsync();

            ViewBag.Activities = activities;
            ViewBag.Nutrients = nutrients;
            var currentUser = await _userManager.GetUserAsync(User);
            return View(currentUser);
        }

        [HttpPost]
        public async Task<IActionResult> ManageDetails(ApplicationUser user)
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
            }

            return RedirectToAction("ManageDetails");
        }
    }
}
