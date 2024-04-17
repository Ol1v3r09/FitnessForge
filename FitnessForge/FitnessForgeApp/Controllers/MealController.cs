using FitnessForgeApp.Data;
using FitnessForgeApp.Models;
using FitnessForgeApp.Models.ViewModels;
using FitnessForgeApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Packaging.Signing;
using System.Globalization;

namespace FitnessForgeApp.Controllers
{
    [Authorize(Roles = "User")]
    public class MealController : Controller
    {
        ApplicationDbContext db;
        UserManager<ApplicationUser> userManager;

        public MealController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
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
                var model = new MealEditIntakeViewModel();

                var currentUser = await userManager.GetUserAsync(User);

                if (currentUser == null)
                {
                    ViewData["ErrorMessage"] = "A felhasználó nem található";
                    return View();
                }
                
                model.userMeals = await (from m in db.meals
                                       where m.DailyIntake.Date == DateTime.Today &&
                                             m.DailyIntake.UserId == currentUser.Id &&
                                             m.MealType.Name == mealType
                                       select m)
                                       .Include(m => m.Food)
                                       .Include(m => m.MealType)
                                       .ToListAsync();

                model.userMealFoodIds = (from m in model.userMeals select m.FoodId).ToList();

                model.userMealsFoodHasProducts = await db.foodsHasProducts.Where(f => model.userMealFoodIds.Contains(f.FoodId)).ToListAsync();

                var a = db.units.ToList();

                foreach (var f in model.userMealsFoodHasProducts)
                {
                    double fhpAmount = 0.0;
                    double fAmount = 0.0;
                    foreach (var p in f.Food.Products)
                    {
                        fhpAmount = f.Amount;
                        if (f.Product.Unit.Name.ToLower().Contains("gramm"))
                        {
                            fhpAmount = UnitConverter.ConvertMass(fhpAmount, f.Product.Unit.Name, "Gramm");
                        }
                        if (f.Product.Unit.Name.ToLower().Contains("liter"))
                        {
                            fhpAmount = UnitConverter.ConvertVolume(fhpAmount, f.Product.Unit.Name, "Milliliter");
                        }

                    }
                    f.Amount = fhpAmount;
                }

                model.mealType = mealType;

                return View(model);
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
            var viewModel = new MealAddViewModel();
            viewModel.allFoodHasProducts = await db.foodsHasProducts
                .Include(fhp => fhp.Food)
                .Include(fhp => fhp.Product)
                .Where(fhp => fhp.Product.ProductStatus == "Jóváhagyva" && !foods.Contains(fhp.FoodId))
                .GroupBy(fhp => fhp.FoodId)
                .Select(group => group.First())
                .ToListAsync();

            foreach (var f in viewModel.allFoodHasProducts)
            {
                double fhpAmount = 0.0;
                double fAmount = 0.0;
                foreach (var p in f.Food.Products)
                {
                    fhpAmount = f.Amount;
                    if (f.Product.Unit.Name.ToLower().Contains("gramm"))
                    {
                        fhpAmount = UnitConverter.ConvertMass(fhpAmount, f.Product.Unit.Name, "Gramm");
                    }
                    if (f.Product.Unit.Name.ToLower().Contains("liter"))
                    {
                        fhpAmount = UnitConverter.ConvertVolume(fhpAmount, f.Product.Unit.Name, "Milliliter");
                    }
                }
                f.Amount = fhpAmount;
            }

            viewModel.foods = foods.ToList();

            viewModel.mealType = mealType;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddFoodToMeal(int FoodId, double Amount, string mealType)
        {
            try
            {
                var currentUser = await userManager.GetUserAsync(User);

                var userDailyIntake = await db.dailyIntakes
                    .Where(x => x.UserId == currentUser.Id && x.Date == DateTime.Today)
                    .FirstOrDefaultAsync();

                var currentMealType = await db.mealTypes
                    .Where(x => x.Name == mealType)
                    .FirstOrDefaultAsync();

                var currenFood = await db.foods.FirstOrDefaultAsync(x => x.Id == FoodId);

                var newMeal = new Meal
                {
                    MealTypeId = currentMealType.Id,
                    IntakeId = userDailyIntake.Id,
                    DailyIntake = userDailyIntake,
                    FoodId = currenFood.Id,
                    Food = currenFood,
                    MealType = currentMealType,
                };

                var newMealAmount = Amount / db.foods.Where(x => x.Id == FoodId).Select(x => x.Amount).First();

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
                var sameBrandPlusName = await db.products.Where(x => x.Brand + " " + x.Name == product.Brand + " " + product.Name).FirstOrDefaultAsync();
                if (sameBrandPlusName != null)
                {
                    ViewData["ErrorMessage"] = "Ilyen márkájú és nevű termék már létezik";
                    return RedirectToAction("CreateProduct");
                }
                else
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
                        BarCode = product.BarCode,
                        ProductStatus = product.ProductStatus
                    };

                    newProduct.Unit = await db.units.Where(x => x.Id == product.UnitId).FirstAsync();

                    await db.products.AddAsync(newProduct);

                    var newFood = new Food
                    {
                        Name = newProduct.Brand + " - " + newProduct.Name,
                        Unit = newProduct.Unit,
                        UnitId = newProduct.UnitId,
                    };

                    newFood.Products.Add(newProduct);

                    await db.foods.AddAsync(newFood);

                    await db.SaveChangesAsync();

                    var foodHasProduct = await db.foodsHasProducts.Where(x => x.ProductId == newProduct.Id && x.FoodId == newFood.Id).FirstAsync();
                    foodHasProduct.Amount = Amount;

                    db.foodsHasProducts.Update(foodHasProduct);

                    newFood = await db.foods.FirstAsync(x => x.Id == foodHasProduct.FoodId);
                    newFood.Amount = Amount;

                    db.foods.Update(newFood);

                    await db.SaveChangesAsync();
                    return RedirectToAction("CreateProduct");
                }
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

                var allProducts = await db.products.Where(p => p.ProductStatus == "Jóváhagyva").ToListAsync();

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

                    var product = await db.products.FindAsync(productId);

                    var foodHasProduct = await db.foodsHasProducts.FindAsync(newFood.Id, product.Id);

                    if (double.TryParse(receiptAmounts[i], NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                    {
                        await Console.Out.WriteLineAsync("Hiba a double parse-nál");
                    }

                    foodHasProduct.Amount = result;
                }

                await db.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba: " + ex.Message);

                ViewData["ErrorMessage"] = "Hiba lépett fel az étel létrehozása közben";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchAddFood(string searchString, string mealType, List<int> foods)
        {
            try
            {
                var viewModel = new MealAddViewModel
                {
                    allFoodHasProducts = new List<FoodHasProduct>(),
                    mealType = mealType,
                    foods = foods
                };

                var unfiltered = await db.foodsHasProducts.Where(f => !foods.Contains(f.FoodId) && f.Product.ProductStatus == "Jóváhagyva").ToListAsync();

                if (searchString != null)
                {
                    viewModel.allFoodHasProducts = unfiltered.Where(f => f.Food.Name.Contains(searchString)).ToList();
                }
                else
                {
                    viewModel.allFoodHasProducts = unfiltered.ToList();
                }

                foreach (var f in viewModel.allFoodHasProducts)
                {
                    double fhpAmount = 0.0;
                    double fAmount = 0.0;
                    foreach (var p in f.Food.Products)
                    {
                        fhpAmount = f.Amount;
                        if (f.Product.Unit.Name.ToLower().Contains("gramm"))
                        {
                            fhpAmount = UnitConverter.ConvertMass(fhpAmount, f.Product.Unit.Name, "Gramm");
                        }
                        if (f.Product.Unit.Name.ToLower().Contains("liter"))
                        {
                            fhpAmount = UnitConverter.ConvertVolume(fhpAmount, f.Product.Unit.Name, "Milliliter");
                        }

                    }
                    f.Amount = fhpAmount;
                }

                return PartialView("_AddFoodPartial", viewModel);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error occurred.");
            }
        }



        [HttpGet]
        public async Task<IActionResult> SearchFood(string searchString, string mealType)
        {
            try
            {
                var viewModel = new MealEditIntakeViewModel();

                var currentUser = await userManager.GetUserAsync(User);
                viewModel.userMeals = await (from m in db.meals
                                      where m.DailyIntake.Date == DateTime.Today &&
                                            m.DailyIntake.UserId == currentUser.Id &&
                                            m.MealType.Name == mealType
                                      select m)
                       .Include(m => m.Food)
                       .Include(m => m.Food.Products)
                       .Include(m => m.MealType)
                       .ToListAsync();

                if (!string.IsNullOrEmpty(searchString))
                {
                    viewModel.userMeals = viewModel.userMeals.Where(m => m.Food.Name.Contains(searchString)).ToList();
                }

                viewModel.userMealFoodIds = (from m in viewModel.userMeals select m.FoodId).ToList();

                viewModel.userMealsFoodHasProducts = await db.foodsHasProducts.Where(f => viewModel.userMealFoodIds.Contains(f.FoodId)).ToListAsync();

                foreach (var f in viewModel.userMealsFoodHasProducts)
                {
                    double fhpAmount = 0.0;
                    double fAmount = 0.0;
                    foreach (var p in f.Food.Products)
                    {
                        fhpAmount = f.Amount;
                        if (f.Product.Unit.Name.ToLower().Contains("gramm"))
                        {
                            fhpAmount = UnitConverter.ConvertMass(fhpAmount, f.Product.Unit.Name, "Gramm");
                        }
                        if (f.Product.Unit.Name.ToLower().Contains("liter"))
                        {
                            fhpAmount = UnitConverter.ConvertVolume(fhpAmount, f.Product.Unit.Name, "Milliliter");
                        }

                    }
                    f.Amount = fhpAmount;
                }

                viewModel.mealType = mealType;

                return PartialView("_FoodPartial", viewModel);
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
                    .Where(p => !productIds.Contains(p.Id) && p.ProductStatus == "Jóváhagyva")
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

        [HttpGet]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> ManageProduct()
        {
            var products = await db.products.Where(p => p.ProductStatus == "Jóváhagyásra vár").ToListAsync();
            return View(products);
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> ProductAllow(int productId)
        {
            var product = await db.products.FirstOrDefaultAsync(p => p.Id == productId);
            product.ProductStatus = "Jóváhagyva";
            db.Update(product);
            await db.SaveChangesAsync();
            return RedirectToAction("ManageProduct");
        }

        [HttpPost]
        [Authorize(Roles = "Manage, Admin")]
        public async Task<IActionResult> ProductForbid(int productId)
        {
            var product = await db.products.FirstOrDefaultAsync(p => p.Id == productId);
            var foodHasProduct = await db.foodsHasProducts.FirstOrDefaultAsync(p => p.ProductId == productId);
            var food = await db.foods.FirstOrDefaultAsync(f => f.Id == foodHasProduct.FoodId);

            db.products.Remove(product);
            db.foodsHasProducts.Remove(foodHasProduct);
            db.foods.Remove(food);
            await db.SaveChangesAsync();
            return RedirectToAction("ManageProduct");
        }
    }
}