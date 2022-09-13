using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using PeninsulaPhysiotherapy.Models;
using System.Data;

namespace PeninsulaPhysiotherapy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;
        public UsersController(
            RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager)
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
                UserName = user.UserName,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserVM editUserVM)
        {
            var user = await userManager.FindByIdAsync(editUserVM.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {editUserVM.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.UserName = editUserVM.UserName;
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(editUserVM);
            }
        }



        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            var model = new List<RoleUserVM>();
            foreach(var role in roleManager.Roles)
            {
                var roleUserVM = new RoleUserVM
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    roleUserVM.IsSelected = true;
                }
                else
                {
                    roleUserVM.IsSelected = false;
                }
                model.Add(roleUserVM);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<RoleUserVM> model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove role");
                return View(model);
            }
            result = await userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add role");
                return View(model);
            }
            return RedirectToAction("EditUser", new {Id=userId});
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
