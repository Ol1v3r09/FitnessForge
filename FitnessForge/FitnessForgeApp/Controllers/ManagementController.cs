using FitnessForgeAdmin.Models.Contexts;
using FitnessForgeApp.Models;
using FitnessForgeApp.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessForgeApp.Controllers
{
    public class ManagementController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<ApplicationUser> _userManager;

        public ManagementController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        //GET: Management/Users
        public async Task<IActionResult> Users()
        {
            var userRoles = new List<UserRolesViewModel>();
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var userWithRolesView = new UserRolesViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Roles = roles.ToList()
                };

                userRoles.Add(userWithRolesView);
            }
            ViewBag.Users = userRoles;
            return View();
        }

        // POST: Management/DeleteUser
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            await _userManager.DeleteAsync(user);

            return RedirectToAction("Users");
        }

        // GET: Management/Roles
        public ActionResult Roles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        // GET: Role/Create
        public ActionResult CreateRoles()
        {
            return View();
        }

        // POST: Management/Create
        [HttpPost]
        public async Task<IActionResult> CreateRoles(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
            }
            return View(role);
        }

        // GET: Management/AssignRole visszaadja az összes User-t és Role-t
        public async Task<IActionResult> AssignRole()
        {
            var users = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();

            ViewBag.Users = users;
            ViewBag.Roles = roles;

            return View();
        }

        // POST: Management/AssignRole Post action hogy lekezeljük a form-ot
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var role = await _roleManager.FindByNameAsync(roleName);

            if (user != null && role != null)
            {
                await _userManager.AddToRoleAsync(user, role.Name);
            }

            return RedirectToAction("Roles");
        }
    }
}
