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
            ViewBag.UserRole = new Dictionary<string, IList<string>>();
            foreach (var user in users)
            {
                var userList = await userManager.GetRolesAsync(user);
                ViewBag.UserRole.Add(user.UserName, userList);
            }
            return View(users);
        }

        public IActionResult EditUser()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }
            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);
            var model = new EditUserVM()
            {
                Id = user.Id,
                Email = user.UserName,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles
            };
            return View(model);
        }


        public async Task<IActionResult> DeleteUser(string id)
        {
            var user= await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("ListUsers");
            }
        }

    }
}
