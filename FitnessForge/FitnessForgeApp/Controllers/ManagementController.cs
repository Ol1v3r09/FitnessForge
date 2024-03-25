using FitnessForgeAdmin.Models.Contexts;
using FitnessForgeApp.Models;
using FitnessForgeApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessForgeApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManagementController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<ApplicationUser> _userManager;

        public ManagementController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            try
            {
                List<UserWithRolesViewModel> userWithRolesList = new List<UserWithRolesViewModel>();

                List<ApplicationUser> allUsers = _userManager.Users.ToList();
                if (allUsers == null)
                {
                    ViewData["ErrorMessage"] = "Nem található felhasználó";
                    //Le kell kezelni majd valahogy
                    return View();
                }

                foreach (ApplicationUser user in allUsers)
                {
                    var allRolesOfUser = await _userManager.GetRolesAsync(user);
                    if (allRolesOfUser == null)
                    {
                        ViewData["ErrorMessage"] = $"Nem sikerült lekérni a felhasználó szerepköreit. Azonosytó: {user.Id}";
                        return View();
                    }

                    UserWithRolesViewModel userWithRoles = new UserWithRolesViewModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Roles = allRolesOfUser.ToList()
                    };

                    userWithRolesList.Add(userWithRoles);
                }

                ViewData["UsersWithRoles"] = userWithRolesList;
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba: {ex.Message}");

                ViewData["ErrorMessage"] = "Hiba lépett fel a kérés feldolgozása közben";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string UserId)
        {
            try
            {
                var UserToBeDeleted = await _userManager.FindByIdAsync(UserId);
                if (UserToBeDeleted == null)
                {
                    ViewData["ErrorMessage"] = "A felhasználó nem található";
                    return RedirectToAction("Users");
                }

                await _userManager.DeleteAsync(UserToBeDeleted);

                return RedirectToAction("Users");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba: {ex.Message}");

                ViewData["ErrorMessage"] = "Hiba lépett fel a felhasználó törlése közben";
                return RedirectToAction("Users");
            }
        }

        [HttpGet]
        public ActionResult Roles()
        {
            var allRoles = _roleManager.Roles;
            if(allRoles == null)
            {
                ViewData["ErrorMessage"] = "Nem találhatók szerepek";
                return View();
            }
            return View(allRoles);
        }

        [HttpGet]
        public ActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(IdentityRole role)
        {
            try
            {
                await _roleManager.CreateAsync(role);
                return RedirectToAction("Roles");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba: {ex.Message}");

                ViewData["ErrorMessage"] = "Hiba lépett fel a szerep létrehozása közben";
                return RedirectToAction("Roles");
            }
        }

        [HttpGet]
        public async Task<IActionResult> AssignRole()
        {
            try
            {
                var allUsers = await _userManager.Users.ToListAsync();
                var allRoles = await _roleManager.Roles.ToListAsync();

                ViewData["allUsers"] = allUsers;
                ViewData["allRoles"] = allRoles;

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba: {ex.Message}");

                ViewData["ErrorMessage"] = "Hiba lépett fel a felhasználók és a szerepek lekérése közben";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                var role = await _roleManager.FindByNameAsync(roleName);

                if (user == null)
                {
                    ViewData["ErrorMessage"] = "Nem található a felhasználó";
                    return RedirectToAction("Roles");
                }

                if (role == null)
                {
                    ViewData["ErrorMessage"] = "Nem található a szerep";
                    return RedirectToAction("Roles");
                }

                await _userManager.AddToRoleAsync(user, role.Name);

                return RedirectToAction("Roles");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba: {ex.Message}");

                ViewData["ErrorMessage"] = "Hiba lépett fel a szerep felhasználóhoz való rendelése közben";
                return RedirectToAction("Roles");
            }
        }
    }
}
