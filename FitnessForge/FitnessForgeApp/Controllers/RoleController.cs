using FitnessForgeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessForgeApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: Role/Index
        public ActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        // GET: Role/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        [HttpPost]
        public async Task<ActionResult> Create(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(role);
        }

        // GET: Role/Create visszaadja az összes User-t és Role-t
        public async Task<IActionResult> AssignRole()
        {
            var users = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();

            ViewBag.Users = users;
            ViewBag.Roles = roles;

            return View();
        }

        // POST: Role/AssignRole Post action hogy lekezeljük a form-ot
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var role = await _roleManager.FindByNameAsync(roleName);

            if (user != null && role != null)
            {
                await _userManager.AddToRoleAsync(user, role.Name);
            }

            var users = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();

            ViewBag.Users = users;
            ViewBag.Roles = roles;

            return View();
        }
    }
}
