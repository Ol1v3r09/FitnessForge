using FitnessForgeAdmin.Models.Contexts;
using FitnessForgeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
            List<Unit> u = db.units.ToList();
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

                var userMeals = await (from m in db.meals
                                       where m.DailyIntake.Date == DateTime.Today &&
                                             m.DailyIntake.UserId == currentUser.Id &&
                                             m.MealType.Name == mealType
                                       select m)
                                       .Include(m => m.Food)
                                       .Include(m => m.MealType)
                                       .ToListAsync();

                ViewData["mealType"] = mealType;

                return View(userMeals);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba: {ex.Message}");

                ViewData["ErrorMessage"] = "Hiba lépett fel az ételek lekérése közben";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string mealType, int foodId)
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

        [HttpPost]
        public async Task<IActionResult> Add(List<int>? foods, string mealType)
        {
            var allFoods = await db.foods.ToListAsync();
            if (foods != null)
            {
                allFoods = await db.foods.Where(x => !foods.Contains(x.Id)).ToListAsync();
            }
            ViewData["mealType"] = mealType;
            ViewData["allFoods"] = allFoods;

            return View();
        }

        [HttpPost]
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
                };
                
                var newMealAmount = meal.Amount / db.foods.Where(x => x.Id == meal.FoodId).Select(x => x.Amount).First();
                newMeal.Amount = newMealAmount;

                await db.meals.AddAsync(newMeal);
                await db.SaveChangesAsync();

                return RedirectToAction("EditIntake", new { mealType = currentMealType.Name });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba: {ex.InnerException}");

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
        public async Task<IActionResult> CreateProduct(Product product, int Amount)
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
                    UnitId = product.UnitId,
                    BarCode = product.BarCode
                };

                newProduct.Unit = await db.units.Where(x => x.Id == product.UnitId).FirstAsync();

                await db.products.AddAsync(newProduct);

                var newFood = new Food
                {
                    Name = product.Brand + " - " + product.Name,
                    Unit = newProduct.Unit,
                    UnitId = newProduct.UnitId,
                };

                newFood.Products.Add(newProduct);

                await db.foods.AddAsync(newFood);

                await db.SaveChangesAsync();

                var foodHasProduct = await db.foodsHasProducts.Where(x => x.ProductId == newProduct.Id && x.FoodId == newFood.Id).FirstAsync();
                foodHasProduct.Amount = Amount;

                newFood.Amount = Amount;
                db.foods.Update(newFood);

                await db.SaveChangesAsync();

                return RedirectToAction("CreateProduct");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba: {ex.InnerException}");
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
                var foodAmount = double.Parse(form["foodAmount"]);

                Food newFood = new Food
                {
                    Name = name,
                    Instructions = instructions,
                    UnitId = unitId,
                    Amount = foodAmount
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

        [HttpGet]
        public async Task<IActionResult> SearchFood(string searchString, string mealType)
        {
            try
            {
                var currentUser = await userManager.GetUserAsync(User);
                var meals = (from m in db.meals
                                      where m.DailyIntake.Date == DateTime.Today &&
                                            m.DailyIntake.UserId == currentUser.Id &&
                                            m.MealType.Name == mealType
                                      select m)
                       .Include(m => m.Food)
                       .Include(m => m.Food.Products)
                       .Include(m => m.MealType)
                       .AsQueryable();

                if (!string.IsNullOrEmpty(searchString))
                {
                    meals = meals.Where(m => m.Food.Name.Contains(searchString));
                }

                if (meals == null)
                {
                    return PartialView("_FoodPartial", new List<Meal>());
                }

                return PartialView("_FoodPartial", meals.ToList());
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error occurred.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchProduct(string search, List<int> productIds)
        {
            try
            {
                var products = await db.products
                    .Where(p => !productIds.Contains(p.Id))
                    .ToListAsync();

                if (!string.IsNullOrEmpty(search))
                {
                    products = products
                        .Where(p => $"{p.Brand} - {p.Name}".Contains(search))
                        .ToList();
                }

                if (products == null)
                {
                    return PartialView("_ProductPartial", new List<Product>());
                }

                return PartialView("_ProductPartial", products);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error occurred.");
            }
        }
    }
}