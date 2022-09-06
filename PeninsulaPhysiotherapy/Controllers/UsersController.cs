using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeninsulaPhysiotherapy.Models;
using System.Data;

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
        public async Task<IActionResult> ListUsers(string id)
        {
            var users = userManager.Users;
            if (!String.IsNullOrEmpty(id))
            {
                users = users.Where(s => s.UserName!.Contains(id));
                ViewBag.Search = $"search result for '{id}'";
            }
            var roles = roleManager.Roles;
            ViewBag.UserRole = new Dictionary<string, List<string>>();
            foreach(var user in users)
            {
                var roleList = new List<string>();
                foreach (var role in roles)
                {
                    if(await userManager.IsInRoleAsync(user, role.Name))
                    {
                        roleList.Add(role.Name);
                    }
                }

                ViewBag.UserRole.Add(user.UserName, roleList);
            }
            return View(users);
        }

        public IActionResult EditUser()
        {
                return View();
        }
    }
}
