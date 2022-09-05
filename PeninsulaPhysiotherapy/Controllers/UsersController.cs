using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeninsulaPhysiotherapy.Models;

namespace PeninsulaPhysiotherapy.Controllers
{
    public class UsersController:Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        public UsersController(
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult ListUsers()
        {
            var user = userManager.Users;
            return View(user);
        }
    }
}
