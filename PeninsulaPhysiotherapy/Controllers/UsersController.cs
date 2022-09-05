using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PeninsulaPhysiotherapy.Controllers
{
    public class UsersController : Controller
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
        public IActionResult ListUsers(string id)
        {
            var users = userManager.Users;
            if (!String.IsNullOrEmpty(id))
            {
                users = users.Where(s => s.UserName!.Contains(id));
                ViewBag.Search = $"search result for '{id}'";
            }
            return View(users);
        }
    }
}
