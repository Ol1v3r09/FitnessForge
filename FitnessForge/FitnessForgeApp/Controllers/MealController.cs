using FitnessForgeAdmin.Models.Contexts;
using FitnessForgeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessForgeApp.Controllers
{
    [Authorize(Roles = "User")]
    public class MealController : Controller
    {
        UserMealContext db;
        UserManager<ApplicationUser> userManager;

        public MealController(UserMealContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;

            //Lazy loading miatt kell
            List<FoodHasProduct> f = db.foodsHasProducts.ToList();
            List<Food> food = db.foods.ToList();
            List<Product> p = db.products.ToList();
        }

        [HttpGet]
        public async Task<IActionResult> EditIntake(string mealType)
        {
            try
            {
                var currentUser = await userManager.GetUserAsync(User);

                if (currentUser == null)
                {
                    ViewData["ErrorMessage"] = "A felhasználó nem található";
                    return View();
                }

                var userFoods = await (from m in db.meals
                                   where m.DailyIntake.Date == DateTime.Today &&
                                         m.DailyIntake.UserId == currentUser.Id &&
                                         m.MealType.Name == mealType
                                   select m.Food).ToListAsync();

                ViewData["mealType"] = mealType;

                return View(userFoods);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba: {ex.Message}");

                ViewData["ErrorMessage"] = "Hiba lépett fel az ételek lekérése közben";
                return View();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string mealType,int foodId)
        {
            try
            {
                var currentUser = await userManager.GetUserAsync(User);

                if (currentUser == null)
                {
                    ViewData["ErrorMessage"] = "A felhasználó nem található";
                    return RedirectToAction("EditIntake", new { mealType = mealType });
                }

                var mealToBeDeleted = await (from m in db.meals
                                  where m.MealType.Name == mealType &&
                                        m.DailyIntake.UserId == currentUser.Id &&
                                        m.DailyIntake.Date == DateTime.Today &&
                                        m.FoodId == foodId
                                  select m).FirstOrDefaultAsync();

                if (mealToBeDeleted != null)
                {
                    db.meals.Remove(mealToBeDeleted);
                    await db.SaveChangesAsync();
                }

                return RedirectToAction("EditIntake", new { mealType = mealType });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba: {ex.Message}");

                ViewData["ErrorMessage"] = "Hiba lépett fel az étkezés törlése közben";
                return RedirectToAction("EditIntake", new { mealType = mealType });
            }

        }

        [HttpGet]
        public async Task<IActionResult> Add(string mealType)
        {
            try
            {
                var allFoods = await db.foods.ToListAsync();

                ViewData["mealType"] = mealType;
                ViewData["allFoods"] = allFoods;

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba: {ex.Message}");

                ViewData["ErrorMessage"] = "Hiba lépett fel az ételek lekérése közben";
                return View();
            }

        }

        [HttpGet]
        public async Task<IActionResult> AddFoodToMeal(Meal meal, string mealType)
        {
            try
            {
                var currentUser = await userManager.GetUserAsync(User);

                if (currentUser == null)
                {
                    ViewData["ErrorMessage"] = "A felhasználó nem található";
                    return RedirectToAction("Index", "Home");
                }

                var userDailyIntake = await db.dailyIntakes
                    .Where(x => x.UserId == currentUser.Id && x.Date == DateTime.Today)
                    .FirstOrDefaultAsync();

                if (userDailyIntake == null)
                {
                    ViewData["ErrorMessage"] = "A napi beivtel nem található";
                    return RedirectToAction("Index", "Home");
                }

                var currentMealType = await db.mealTypes
                    .Where(x => x.Name == mealType)
                    .FirstOrDefaultAsync();

                if (currentMealType == null)
                {
                    ViewData["ErrorMessage"] = "Az étkezés típus nem található";
                    return RedirectToAction("Index", "Home");
                }

                var newMeal = new Meal
                {
                    MealTypeId = currentMealType.Id,
                    IntakeId = userDailyIntake.Id,
                    DailyIntake = userDailyIntake,
                    FoodId = meal.FoodId,
                    Food = meal.Food,
                    MealType = currentMealType,
                    Amount = meal.Amount
                };

                await db.meals.AddAsync(newMeal);
                await db.SaveChangesAsync();

                return RedirectToAction("EditIntake", new { mealType = currentMealType.Name });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba: {ex.Message}");

                ViewData["ErrorMessage"] = "Hiba lépett fel az étkezés létrehozásakor";
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            try
            {
                var allUnits = await db.units.ToListAsync();

                ViewData["allUnits"] = allUnits;

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba: {ex.Message}");

                ViewData["ErrorMessage"] = "Hiba lépett fel a mértékegységek lekérésekor";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            try
            {
                var newProduct = new Product
                {
                    Brand = product.Brand,
                    Name = product.Name,
                    Calorie = product.Calorie,
                    Carbohydrate = product.Carbohydrate,
                    Sugar = product.Sugar,
                    Protein = product.Protein,
                    Fat = product.Fat,
                    SaturatedFat = product.SaturatedFat,
                    Salt = product.Salt,
                    Fiber = product.Fiber,
                    UnitId = product.UnitId
                };

                newProduct.Unit = await db.units.FirstOrDefaultAsync(u => u.Id == newProduct.UnitId);

                await db.products.AddAsync(newProduct);

                var newFood = new Food
                {
                    Name = product.Brand + " - " + product.Name,
                    Unit = newProduct.Unit,
                    UnitId = newProduct.UnitId
                };

                newFood.Products.Add(newProduct);

                await db.foods.AddAsync(newFood);

                await db.SaveChangesAsync();

                return RedirectToAction("CreateProduct");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba: {ex.Message}");
                ViewData["ErrorMessage"] = "Hiba lépett fel a termék létrehozása közben";
                return RedirectToAction("CreateProduct");
            }

        }

        [HttpGet]
        public async Task<IActionResult> CreateFood()
        {
            try
            {
                var allUnits = await db.units.ToListAsync();

                var allProducts = await db.products.ToListAsync();

                ViewData["allProducts"] = allProducts;
                ViewData["allUnits"] = allUnits;

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba: {ex.Message}");

                ViewData["ErrorMessage"] = "Hiba lépett fel az ételek és a mértékegységek lekérése közben";
                return View();
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateFood(IFormCollection form)
        {
            try
            {
                var name = form["Name"];
                var instructions = form["Instructions"];
                var unitId = int.Parse(form["unit"]);
                var receiptProductIds = form["receiptProductId"];
                var receiptAmounts = form["receiptAmount"];

                Food newFood = new Food
                {
                    Name = name,
                    Instructions = instructions,
                    UnitId = unitId
                };

                newFood.Unit = await db.units.FindAsync(unitId);

                newFood.Products = await db.products.Where(p => receiptProductIds.Contains(p.Id.ToString())).ToListAsync();

                await db.foods.AddAsync(newFood);
                await db.SaveChangesAsync();

                for (int i = 0; i < receiptProductIds.Count; i++)
                {
                    var productId = int.Parse(receiptProductIds[i]);
                    var amountStr = receiptAmounts[i];

                    if (!double.TryParse(amountStr, out var amount))
                    {
                        Console.WriteLine("Nem megfelelő mennyiség a terméknél Id: " + productId);
                        continue;
                    }

                    var product = await db.products.FindAsync(productId);

                    var foodHasProduct = await db.foodsHasProducts.FindAsync(newFood.Id, product.Id);
                    foodHasProduct.Amount = amount;
                }

                await db.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba: " + ex.Message);

                ViewData["ErrorMessage"] = "Hiba lépett fel az étel létrehozása közben";
                return View();
            }

        }
    }
}